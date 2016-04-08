using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.Interfaces;
using Repeat.SyncronizerService.DAL;
using Repeat.SyncronizerService.DTOs;
using Repeat.SyncronizerService.APICallers;
using System.Configuration;

namespace Repeat.SyncronizerService
{
	public class Syncher
	{
		public static void Main(string[] args)
		{
			new Syncher().Launch();
		}

		private IQueue _queue;
		private IUnitOfWork _unitOfWork;
		private CentralAPICaller _apiCaller;

		public Syncher()
		{
			_unitOfWork = new UnitOfWork();
			_apiCaller = new CentralAPICaller();
		}

		public void Launch()
		{


			using (_queue = new RabbitMQ())
			{
				_queue.ProcessMessage<SyncRequest>(Config.RabbitMQ_PrepareSyncQueue, Process_SyncRequestQueue);

				_queue.ProcessMessage<DataToBeSynched>(Config.RabbitMQ_DataToBeSynchedQueue, Process_DataToBeSynchedQueue);

				Console.WriteLine("Press any key to exit!");
				Console.ReadKey();
			}
		}

		//TODO:: check if strategy pattern might work here
		//TODO:: implement json schema for all dtos
		public void Process_SyncRequestQueue(SyncRequest message)
		{
			UserLastSync userLastSync = _unitOfWork.UsersLastSyncRepository.GetUserLastSyncFor(message);

			_queue.SendMessage(Config.RabbitMQ_SyncRequestQueue, new SyncRequestResponse()
			{
				UserId = userLastSync.UserId,
				Device = message.Device,
				LastSyncDate = userLastSync.LastSyncDate,
			});
		}
		//TODO::seems like values inside db are cached..so every time one of the processors gets called a new UnitOfWork instance should be created
		public void Process_DataToBeSynchedQueue(DataToBeSynched message)
		{
			UserLastSync userLastSync = _unitOfWork.UsersLastSyncRepository
				.GetUserLastSyncFor(
				new SyncRequest()
				{
					UserId = message.UserId,
					Device = message.Device,
				});

			if (userLastSync.SyncStatus == DAL.Enums.SyncStatus.Stopped)
			{
				ChangeSyncStatus(userLastSync, DAL.Enums.SyncStatus.Running);

				Task.Factory.StartNew(async () =>
				{
					List<Note> notesToBeUpdated = new List<Note>();
					List<Note> notesToBeInserted = new List<Note>();

					foreach (var notebook in message.Notebooks)
					{
						//todo:: CHECK iF notebooks exists -> if not simply add it
						Notebook notebookApi = await _apiCaller.NotebookAPICaller.GetById(Config.NotebookAPI_Notes_GET, notebook.Id);
						if (notebookApi == null)
						{
							_apiCaller.NotebookAPICaller.Add(Config.NotebookAPI_Notes_GET, notebook);
							continue;
						}
						//TODO::Here I could use the notebookAPI object and check on it for notes changed after LastSyncDate
						//or the api/notebooks/36A21286-5167-43F1-BC74-1C66471254EA route shouldn't offer the notes of the notebook 

						List<Note> apiNotes = await _apiCaller.NotebookAPICaller.GetNotes(Config.NotebookAPI_Notebooks_GET_Notes, notebook.Id, userLastSync.LastSyncDate);

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
								Note apiNoteCheck = await _apiCaller.NoteAPICaller.GetById(Config.NotebookAPI_Notes_GET, note.Id);
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
						_apiCaller.NoteAPICaller.Add(Config.NotebookAPI_Notes_POST, note);
					}
					foreach (var note in notesToBeUpdated)
					{
						_apiCaller.NoteAPICaller.Update(Config.NotebookAPI_Notes_PUT, note);
					}
					//TODO::: CHECK RESPONSE BEFORE SENDING MESSAGE
					//if any of the responses is failed the sync process should be done again
					_queue.SendMessage(Config.RabbitMQ_SyncResultQueue, new SyncResult()
					{
						UserId = userLastSync.UserId,
						Device = message.Device,
						Result = false
					});

					ChangeSyncStatus(userLastSync, DAL.Enums.SyncStatus.Stopped);

					//TODO:: change lastSyncDate if sync succesfull
				});
			}
			else//the sync process is already Running for a certain device
			{
				//TODO::put message back on queue
			}
		}

		private void ChangeSyncStatus(UserLastSync userLastSync, DAL.Enums.SyncStatus syncstatus)
		{
			userLastSync.SyncStatus = syncstatus;
			_unitOfWork.UsersLastSyncRepository.Update(userLastSync);//mark the synching status process as finished
			_unitOfWork.Save();
		}
	}
}
