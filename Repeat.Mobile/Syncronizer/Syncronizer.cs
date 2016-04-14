using Newtonsoft.Json;
using Repeat.Mobile.PCL.Common;
using Repeat.Mobile.PCL.DAL;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.Logging;
using Repeat.Mobile.Sync.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;
using Xamarin.StompClient;
using Xamarin.StompClient.Interfaces;

namespace Repeat.Mobile.Sync
{
	public class Syncronizer
	{

		enum SyncStatus
		{
			Finished = 0,
			Started			
		}
		private WebSocket _webSocket;
		private IStompClient _client;
		private IUnitOfWork _unitOfWork;
		private volatile SyncStatus _syncStatus;

		private static Syncronizer _syncher;



		private Syncronizer()
		{
			_unitOfWork = new UnitOfWork();

			_client = new StompClient(Configs.RabbitMQ_StompClient_Address);
		}

		private void StartWebSocketMonitoringTask()
		{
			Task.Factory.StartNew(() =>
			{
				Task.Delay(TimeSpan.FromMinutes(2));//first wait until a connection is made

				//here check if connection is open every x second(should be incremental)
				//if connection is lost reconnect
				double sleepTime = 5;
				while (_syncStatus != SyncStatus.Finished)
				{

					Task.Delay(TimeSpan.FromSeconds(sleepTime)).Wait();
					if (!_client.Connected)
					{
						_client.Dispose();
						_client = new StompClient(Configs.RabbitMQ_StompClient_Address);

						Kernel.Get<ILog>().Info(Guid.Empty, "Atempting to connect again");

						StartSynching();
					}
					else
					{
						Kernel.Get<ILog>().Info(Guid.Empty, "Connection Status: " + _client.Connected);
						sleepTime = sleepTime == 80 ? 5 : sleepTime * 2;
					}
				}
			});
		}

		//Singleton
		public static Syncronizer GetSyncher()
		{
			if (_syncher == null)
			{
				_syncher = new Syncronizer();
			}

			return _syncher;
		}


		public async void StartSynching()
		{
			//TODO:: maybe try a few times...and if it does not work display message to user
			if (await this.Connect())
			{

				//if the process started for the first time
				if (_syncStatus == SyncStatus.Finished)
				{
					StartWebSocketMonitoringTask();
				}
				_syncStatus = SyncStatus.Started;


				//here I should use the logged in user and device specific identification
				SyncRequest request = new SyncRequest()
				{
					UserId = Guid.Empty,
				};

				_client.Subscribe<SyncRequestResponse>(Configs.RabbitMQ_SyncRequestQueue, n =>
				{
					if (n.Device.Equals(request.Device) && n.UserId.Equals(request.UserId))
					{

						//here I should unsubscribe from the SyncReqeustQueue 
						var notebooks = _unitOfWork.NotebooksRepository.GetNotebooksWithNotesByLastModifiedDateForNotes(n.LastSyncDate);

						_client.Publish(Configs.RabbitMQ_DataToBeSynchedQueue, new DataToBeSynched()
						{
							UserId = n.UserId,
							Notebooks = notebooks,
						});
					}
				}, true);

				_client.Subscribe<SyncResult>(Configs.RabbitMQ_SyncResultQueue, syn =>
				{
					Kernel.Get<ILog>().Info(Guid.Empty, "Sync FINISHED");
					//TODO:: display message to the user that the sync was a succes/failure

					_syncStatus = SyncStatus.Finished;
					_client.Dispose();
				}, true);

				_client.Publish(Configs.RabbitMQ_PrepareSyncQueue, request);
			}
		}

		private async Task<bool> Connect()
		{
			if (!_client.Connected)
			{
				var headers = new Dictionary<string, string>()
				{
					{"login", Configs.RabbitMQ_Login },
					{"passcode", Configs.RabbitMQ_Password},
					{"host", Configs.RabbitMQ_Host },
					{"accept-version", Configs.RabbitMQ_AcceptVersion },
					{"heart-beat", "10000,10000" }//I hardcoded 10000 inside the StompClient code
				};
				return await _client.Connect(60, headers,
					() => Kernel.Get<ILog>().Info(Guid.Empty, "CONNECTED to WebSocket."),
					(obj, args) =>
					{

						Kernel.Get<ILog>().Exception(Guid.Empty, args.Exception, "WebSocket ErrorHandler Exception");
						if (args.Exception.Message.ToLower().Contains("the socket is not connected") && _syncStatus == SyncStatus.Started)
						{
							Kernel.Get<ILog>().Info(Guid.Empty, "Reconnecting");

							StartSynching();
						}
					});
			}

			return true;
		}
	}
}
