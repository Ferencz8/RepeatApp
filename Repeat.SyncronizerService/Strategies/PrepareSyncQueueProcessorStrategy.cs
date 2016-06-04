using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.DAL;
using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.DTOs;
using Repeat.SyncronizerService.Interfaces;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Repeat.SyncronizerService.Strategies
{
	//TODO:: implement json schema for all dtos
	public class PrepareSyncQueueProcessorStrategy : IQueueProcessor<SyncRequest>
	{
		public void Process(IQueue queue, SyncRequest messageToBeProcessed)
		{
			Task.Factory.StartNew(() =>
			{
				using (IUnitOfWork unitOfWork = new UnitOfWork())
				{
					SyncRequest messageClone = messageToBeProcessed.Clone();
					Console.ForegroundColor = ConsoleColor.Green;
					Log.Info("STARTED Process Sync Request : " + messageClone.ToString());

					UserLastSync userLastSync = unitOfWork.UsersLastSyncRepository.GetUserLastSyncFor(messageToBeProcessed);

					if (userLastSync == null)
					{
						var device = unitOfWork.DevicesRepository.Get(n => n.Name.Equals("ANDROID"));
						if(device.FirstOrDefault() == null)
						{
							throw new Exception("ANDROID Device is not set");
						}

						userLastSync = new UserLastSync()
						{
							UserId = messageClone.UserId,
							LastSyncDate = DateTime.UtcNow.AddYears(-100),
							SyncStatus = DAL.Enums.SyncStatus.Stopped,
							DeviceId = device.FirstOrDefault().Id,
						};
						unitOfWork.UsersLastSyncRepository.Add(userLastSync);
						unitOfWork.Save();
					}

					queue.SendMessage(Config.RabbitMQ_SyncRequestQueue, new SyncRequestResponse()
					{
						UserId = userLastSync.UserId,
						Device = messageToBeProcessed.Device,
						LastSyncDate = userLastSync.LastSyncDate,
					});
					

					Log.Info("END SENT Sync Request Response: " + userLastSync.LastSyncDate.ToString());
					Console.ForegroundColor = ConsoleColor.White;
				}
			});
		}
	}
}
