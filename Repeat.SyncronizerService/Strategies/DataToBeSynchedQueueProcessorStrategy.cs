using Repeat.SyncronizerService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repeat.SyncronizerService.Interfaces;
using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.DAL;
using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.DAL.Enums;
using Repeat.SyncronizerService.APICallers;

namespace Repeat.SyncronizerService.Strategies
{
	public class DataToBeSynchedQueueProcessorStrategy : IQueueProcessor<DataToBeSynched>
	{
		private static readonly object _object = new object();


		//TODO::seems like values inside db are cached..so every time one of the processors gets called a new UnitOfWork instance should be created
		public void Process(IQueue queue, DataToBeSynched messageToBeProcessed)
		{
			//TODO::if incoming msg has the userId and device id of a task already running ignore message and do not requeue
			using (IUnitOfWork unitOfWorkExternal = new UnitOfWork())
			{

				UserLastSync userLastSync = GetUserLastSync(unitOfWorkExternal, messageToBeProcessed);

				Console.ForegroundColor = ConsoleColor.DarkCyan;
				Log.Info("userLastSync.SyncStatus Status :" + userLastSync.SyncStatus);
				Console.ForegroundColor = ConsoleColor.White;
				if (userLastSync.SyncStatus == SyncStatus.Stopped)
				{
					ChangeSyncStatus(unitOfWorkExternal, userLastSync, SyncStatus.Running);

					RunDataSyncTask(queue, messageToBeProcessed, userLastSync);

				}
				else//the sync process is already Running for a certain device
				{
					//TODO::put message back on queue or use ack
				}
			}
		}

		//TODO::
		//Find a way to guarantee atomicity
		//maybe add the whole synched values to the mq -> and another listener will make the update
		//for each failed response the specific value will be put back on the queue and later tried again
		private void RunDataSyncTask(IQueue queue, DataToBeSynched messageToBeProcessed, UserLastSync userLastSync)
		{
			Task.Factory.StartNew(async () =>
			{
				using (IUnitOfWork unitOfWork = new UnitOfWork())
				{
					var headers = new Dictionary<string, string>() { { "Authorization", messageToBeProcessed.UserToken } };
					CentralAPICaller apiCaller = new CentralAPICaller();
					UserLastSync userLastSyncClone = GetUserLastSyncClone(messageToBeProcessed, userLastSync);
					List<Note> notesToBeUpdated = new List<Note>();
					List<Note> notesToBeInserted = new List<Note>();

					await PopulateListsWithDataToBeSynced(messageToBeProcessed, userLastSync, apiCaller, notesToBeUpdated, notesToBeInserted, headers);


					Sync_NotesToBeInserted(apiCaller, notesToBeInserted, headers);
					Sync_NotesToBeUpdated(apiCaller, notesToBeUpdated, headers);

					Console.ForegroundColor = ConsoleColor.Green;
					Log.Info("END Data To Be Synched processing !");

					//TODO::: CHECK RESPONSE BEFORE SENDING MESSAGE
					//if any of the responses is failed the sync process should be done again
					SendSyncResultReponse(queue, userLastSyncClone);

					Console.ForegroundColor = ConsoleColor.Cyan;
					Log.Info("SENT Sync Result message ");

					ChangeSyncStatus(unitOfWork, userLastSyncClone, SyncStatus.Stopped);

					UpdateLastSyncDate(unitOfWork, userLastSyncClone);

					Console.ForegroundColor = ConsoleColor.White;
				}
			});
		}

		private void SendSyncResultReponse(IQueue queue, UserLastSync userLastSyncClone)
		{
			queue.SendMessage(Config.RabbitMQ_SyncResultQueue, new SyncResult()
			{
				UserId = userLastSyncClone.UserId,
				Device = userLastSyncClone.Device.Name,
				Result = true
			});
		}

		private void Sync_NotesToBeUpdated(CentralAPICaller apiCaller, List<Note> notesToBeUpdated, Dictionary<string, string> headers)
		{
			foreach (var note in notesToBeUpdated)
			{
				apiCaller.NoteAPICaller.Update(string.Format(Config.NotebookAPI_Notes_PUT, note.Id), note, headers);
			}
		}

		private void Sync_NotesToBeInserted(CentralAPICaller apiCaller, List<Note> notesToBeInserted, Dictionary<string, string> headers)
		{
			foreach (var note in notesToBeInserted)
			{
				apiCaller.NoteAPICaller.Add(Config.NotebookAPI_Notes_POST, note, headers);
			}
		}

		private async Task PopulateListsWithDataToBeSynced(DataToBeSynched messageToBeProcessed, UserLastSync userLastSync, CentralAPICaller apiCaller, 
			List<Note> notesToBeUpdated, List<Note> notesToBeInserted, Dictionary<string, string> headers)
		{
			foreach (var notebook in messageToBeProcessed.Notebooks)
			{
				Notebook notebookApi = await apiCaller.NotebookAPICaller.Get(string.Format(Config.NotebookAPI_Notebooks_GETByID, notebook.Id), headers);

				//notebook was added from external source so we directly insert in api
				if (notebookApi == null || notebookApi.Id.Equals(Guid.Empty))
				{

					var nbs = await apiCaller.NotebookAPICaller.GetList(string.Format(Config.NotebookAPI_Notebooks_GET_WithParameters,"", userLastSync.UserId, notebook.Name), headers);

					if(nbs!=null && nbs.Any())//a notebok with the same name  for current user already exists 
					{
						notebook.Name += " Conflict";
					}

					apiCaller.NotebookAPICaller.Add(Config.NotebookAPI_Notebooks_POST, notebook, headers);
					continue;
				}

				SyncNotebooks(userLastSync, apiCaller, notebook, notebookApi);

				List<Note> apiNotes = await apiCaller.NotebookAPICaller.GetNotes(Config.NotebookAPI_Notebooks_GET_Notes, notebook.Id, userLastSync.LastSyncDate,
					headers);

				await SyncNotes(apiCaller, notesToBeUpdated, notesToBeInserted, notebook, apiNotes);
			}
		}

		private async Task SyncNotes(CentralAPICaller apiCaller, List<Note> notesToBeUpdated, List<Note> notesToBeInserted, Notebook notebook, List<Note> apiNotes)
		{
			foreach (Note note in notebook.Notes)
			{
				Note apiNote = apiNotes.FirstOrDefault(n => n.Id.Equals(note.Id));
				if (apiNote != null)//both notes exist in API, and they both have been updated out of sync
				{
					if (apiNote.Deleted && note.Deleted)
					{
						note.Id = Guid.NewGuid();
						notesToBeInserted.Add(note);
					}
					else if (apiNote.Deleted)
					{
						apiNote.Name += " - Conflict Deleted";
						apiNote.Deleted = false;
						apiNote.DeletedDate = null;
						notesToBeUpdated.Add(apiNote);

						note.Id = Guid.NewGuid();
						notesToBeInserted.Add(note);
					}
					else if (note.Deleted)
					{
						note.Deleted = false;
						note.DeletedDate = null;
						note.Name += "Conflict - Delete";
						note.Id = Guid.NewGuid();
						notesToBeInserted.Add(note);
					}
					else {//insert note in api & maybe mark the note as conflict
						note.Id = Guid.NewGuid();
						note.Name += " - Conflict";
						notesToBeInserted.Add(note);
					}
				}
				else
				{
					//if note exists in api -> update else -> insert
					Note apiNoteCheck = await apiCaller.NoteAPICaller.Get(string.Format(Config.NotebookAPI_Notes_GETByID, note.Id));
					if (apiNoteCheck != null)//note was edited on external db only, so we update the one from the api
					{
						notesToBeUpdated.Add(note);
					}
					else // note was only added on external db, it does not exist on api
					{
						notesToBeInserted.Add(note);
					}
				}
			}
		}

		private void SyncNotebooks(UserLastSync userLastSync, CentralAPICaller apiCaller, Notebook notebook, Notebook notebookApi)
		{
			if (notebookApi.ModifiedDate < userLastSync.LastSyncDate && notebook.ModifiedDate > userLastSync.LastSyncDate)//notebook changed only in External db
			{
				apiCaller.NotebookAPICaller.Update(Config.NotebookAPI_Notebooks_PUT, notebook);
			}
			else if(notebookApi.ModifiedDate > userLastSync.LastSyncDate && notebook.ModifiedDate > userLastSync.LastSyncDate)//both notebooks exist. Both, API and external Db notebook have been updated out of sync
			{
				if (notebook.Deleted == true && notebookApi.Deleted == true)
				{
					//this notes should also be synched, or they will get lost on the mobile
				}
				else if (notebook.Deleted)
				{
					notebook.Deleted = false;
					notebook.DeletedDate = null;
					notebook.Name += " - Conflict-Deleted";
					notebook.Id = Guid.NewGuid();
					notebook.Notes.ForEach(n => n.Id = Guid.NewGuid());
					apiCaller.NotebookAPICaller.Add(Config.NotebookAPI_Notebooks_PUT, notebook);
				}
				else if (notebookApi.Deleted)
				{
					notebookApi.Deleted = false;
					notebookApi.DeletedDate = null;
					notebookApi.Name += " - Conflict-Deleted";
					apiCaller.NotebookAPICaller.Update(Config.NotebookAPI_Notebooks_PUT, notebookApi);

					notebook.Id = Guid.NewGuid();
					notebook.Notes.ForEach(n => n.Id = Guid.NewGuid());
					apiCaller.NotebookAPICaller.Add(Config.NotebookAPI_Notebooks_PUT, notebook);
				}
				else//none of the notebooks got deleted
				{
					notebook.Name += " -Conflict";
					notebook.Id = Guid.NewGuid();
					notebook.Notes.ForEach(n => n.Id = Guid.NewGuid());
					apiCaller.NotebookAPICaller.Add(Config.NotebookAPI_Notebooks_PUT, notebook);
				}
			}
		}

		private UserLastSync GetUserLastSyncClone(DataToBeSynched messageToBeProcessed, UserLastSync userLastSync)
		{
			return new UserLastSync()
			{
				UserId = userLastSync.UserId,
				LastSyncDate = userLastSync.LastSyncDate,
				DeviceId = userLastSync.DeviceId,
				Device = new Device()
				{
					Id = userLastSync.DeviceId,
					Name = messageToBeProcessed.Device,
				}
			};
		}

		private void UpdateLastSyncDate(IUnitOfWork unitOfWork, UserLastSync userLastSyncClone)
		{
			lock (_object)
			{
				userLastSyncClone.LastSyncDate = DateTime.UtcNow;
				unitOfWork.UsersLastSyncRepository.Update(userLastSyncClone);
				unitOfWork.Save();
			}
		}

		private UserLastSync GetUserLastSync(IUnitOfWork unitOfWork, DataToBeSynched messageToBeProcessed)
		{
			lock (_object)
			{
				return unitOfWork.UsersLastSyncRepository
				.GetUserLastSyncFor(
				new SyncRequest()
				{
					UserId = messageToBeProcessed.UserId,
					Device = messageToBeProcessed.Device,
				});
			}
		}

		private void ChangeSyncStatus(IUnitOfWork unitOfWork, UserLastSync userLastSync, SyncStatus syncstatus)
		{
			lock (_object)
			{
				userLastSync.SyncStatus = syncstatus;
				unitOfWork.UsersLastSyncRepository.Update(userLastSync);//mark the synching status process as finished
				unitOfWork.Save();
			}
		}
	}
}
