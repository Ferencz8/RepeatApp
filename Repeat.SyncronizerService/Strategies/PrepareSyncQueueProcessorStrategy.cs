﻿using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.DAL;
using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.DTOs;
using Repeat.SyncronizerService.Interfaces;
using System;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.Strategies
{
	//TODO:: implement json schema for all dtos
	public class PrepareSyncQueueProcessorStrategy : IQueueProcessor<SyncRequest>
	{
		//TODO:: here lookup on how to dispose db connection if program stops...maybe if I call Task . Dispose it calls all its objects Dispose methods ??
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

					queue.SendMessage(Config.RabbitMQ_SyncRequestQueue, new SyncRequestResponse()
					{
						UserId = userLastSync.UserId,
						Device = messageToBeProcessed.Device,
						LastSyncDate = userLastSync.LastSyncDate,
					});
					

					Log.Info("END SENT Sync Request Response: " + userLastSync.LastSyncDate.ToString());
				}
			});
		}
	}
}