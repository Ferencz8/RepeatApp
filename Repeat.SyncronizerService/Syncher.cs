using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.Interfaces;
using Repeat.SyncronizerService.DAL;
using Repeat.SyncronizerService.DAL.DTOs;
using Repeat.SyncronizerService.APICallers;
using System.Linq;

namespace Repeat.SyncronizerService
{
	//TODO:: implement a solution for when the sync service might become unavailable
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

			//TODO:: check behaviour when multiple accounts are used on the same type of device - maybe instead of using a Device as a string 
			//an id should be set for each device and it should be sent along when sync request is made. Also this device id should maybe be kept 
			//in UserService db ? or on the specific platform db(if so then a device specification should be made along with the id 
			//eg: Android - 12349687521, Android - 5325252) -> so that when sync process starts this id will be temporarily/permanently stored in the db
			using (IQueue _queue = new RabbitMQ())
			{
				_queue.ProcessMessage<RequestSync>(Config.RabbitMQ_PrepareSyncQueue, Process_PrepareSyncQueue);

				_queue.ProcessMessage<DataInfoToBeSynched>(Config.RabbitMQ_DataQueue, Process_DataQueue);

				Console.WriteLine("Press any key to exit!");
				Console.ReadKey();
			}
		}

		//TODO:: check if strategy pattern might work here
		//TODO:: implement json schema for all dtos
		public void Process_PrepareSyncQueue(RequestSync message)
		{
			UserLastSync userLastSync = _unitOfWork.UsersLastSyncRepository.GetUserLastSyncFor(message);

			_queue.SendMessage(Config.RabbitMQ_Last_Sync_Date, new UserLastSync()
			{
				UserId = userLastSync.UserId,
				LastSyncDate = userLastSync.LastSyncDate,
			});
		}

		public void Process_DataQueue(DataInfoToBeSynched message)
		{
			UserLastSync userLastSync = _unitOfWork.UsersLastSyncRepository
				.GetUserLastSyncFor(
				new RequestSync()
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
						//CHECK iF notebooks exists -> if not simply add it
						Notebook apiNotebook = await _apiCaller.NotebookAPICaller.GetById("",notebook.Id);
						if (apiNotebook != null)
						{
							List<Note> apiNotes = await _apiCaller.NotebookAPICaller.GetNotes("", notebook.Id, userLastSync.LastSyncDate);

							foreach (var apiNote in apiNotes)
							{
								if (notebook.Notes.Exists(n => n.Id.Equals(apiNote.Id)))
								{
									//insert note in api & maybe as extra feature mark the note as conflict

									notesToBeInserted.Add(apiNote);
								}
								else
								{
									//if note exists in api -> update else -> insert
									Note apiNoteCheck = await _apiCaller.NoteAPICaller.GetById("", apiNote.Id);
									if (apiNoteCheck != null)
									{
										notesToBeUpdated.Add(apiNote);
									}
									else
									{
										notesToBeInserted.Add(apiNote);
									}
								}
							}
						}
						else
						{
							_apiCaller.NotebookAPICaller.Add("", notebook);
						}
					}


					//Find a way to guarantee atomicity
					//maybe add the whole synched values to the mq -> and another listener will make the update
					//for each failed response the specific value will be put back on the queue and later tried again
					foreach (var note in notesToBeInserted)
					{
						_apiCaller.NoteAPICaller.Add("", note);
					}
					foreach (var note in notesToBeUpdated)
					{
						_apiCaller.NoteAPICaller.Update("", note);
					}
					//TODO::: CHECK RESPONSE BEFORE SENDING MESSAGE
					//if any of the responses is failed the sync process should be done again
					_queue.SendMessage(Config.RabbitMQ_SyncResultQueue,  new SyncResult() { UserId = userLastSync.UserId , Result = false});

					ChangeSyncStatus(userLastSync, DAL.Enums.SyncStatus.Stopped);
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
		}
	}
}
