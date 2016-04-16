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
				if (userLastSync.SyncStatus == SyncStatus.Stopped)
				{
					ChangeSyncStatus(unitOfWorkExternal, userLastSync, SyncStatus.Running);

					Task.Factory.StartNew(async () =>
					{
						using (IUnitOfWork unitOfWork = new UnitOfWork())
						{

							var apiCaller = new CentralAPICaller();
							UserLastSync userLastSyncClone = new UserLastSync()
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


							List<Note> notesToBeUpdated = new List<Note>();
							List<Note> notesToBeInserted = new List<Note>();

							foreach (var notebook in messageToBeProcessed.Notebooks)
							{
								//todo:: CHECK iF notebook exists -> if not simply add it
								Notebook notebookApi = await apiCaller.NotebookAPICaller.Get(string.Format(Config.NotebookAPI_Notebooks_GETByID, notebook.Id));
								if (notebookApi == null)
								{
									apiCaller.NotebookAPICaller.Add(Config.NotebookAPI_Notes_GET, notebook);
									continue;
								}
								//TODO::Here I could use the notebookAPI object and check on it for notes changed after LastSyncDate
								//or the api/notebooks/36A21286-5167-43F1-BC74-1C66471254EA route shouldn't offer the notes of the notebook 

								List<Note> apiNotes = await apiCaller.NotebookAPICaller.GetNotes(Config.NotebookAPI_Notebooks_GET_Notes, notebook.Id, userLastSync.LastSyncDate);

								foreach (Note note in notebook.Notes)
								{
									if (apiNotes.Exists(n => n.Id.Equals(note.Id)))
									{
										//insert note in api & maybe mark the note as conflict
										note.Id = Guid.NewGuid();
										note.Name += " - Conflict";
										notesToBeInserted.Add(note);
									}
									else
									{
										//if note exists in api -> update else -> insert
										Note apiNoteCheck = await apiCaller.NoteAPICaller.Get(string.Format(Config.NotebookAPI_Notes_GETByID, note.Id));
										if (apiNoteCheck != null)
										{
											notesToBeUpdated.Add(note);
										}
										else
										{
											notesToBeInserted.Add(note);
										}
									}
								}
							}


							//Find a way to guarantee atomicity
							//maybe add the whole synched values to the mq -> and another listener will make the update
							//for each failed response the specific value will be put back on the queue and later tried again
							foreach (var note in notesToBeInserted)
							{
								apiCaller.NoteAPICaller.Add(Config.NotebookAPI_Notes_POST, note);
							}
							foreach (var note in notesToBeUpdated)
							{
								apiCaller.NoteAPICaller.Update(string.Format(Config.NotebookAPI_Notes_PUT,note.Id), note);
							}
							Console.ForegroundColor = ConsoleColor.Green;
							Log.Info("END Data To Be Synched processing !");
							//TODO::: CHECK RESPONSE BEFORE SENDING MESSAGE
							//if any of the responses is failed the sync process should be done again
							queue.SendMessage(Config.RabbitMQ_SyncResultQueue, new SyncResult()
							{
								UserId = userLastSyncClone.UserId,
								Device = userLastSyncClone.Device.Name,
								Result = false
							});
							Console.ForegroundColor = ConsoleColor.Cyan;
							Log.Info("SENT Sync Result message ");
							ChangeSyncStatus(unitOfWork, userLastSyncClone, SyncStatus.Stopped);
							
							UpdateLastSyncDate(unitOfWork, userLastSyncClone);
						}
					});

				}
				else//the sync process is already Running for a certain device
				{
					//TODO::put message back on queue or use ack
				}
			}
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
