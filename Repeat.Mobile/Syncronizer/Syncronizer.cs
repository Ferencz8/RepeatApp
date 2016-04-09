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

namespace Repeat.Mobile.Sync
{
	public class Syncronizer
	{
		private WebSocket _webSocket;
		private StompClient _client;
		private IUnitOfWork _unitOfWork;
		
		private static Syncronizer _syncher;



		private Syncronizer()
		{
			_unitOfWork = new UnitOfWork();

			_client = new StompClient(Configs.RabbitMQ_StompClient_Address);
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
			this.Connect();

			//here I should use the logged in user and device specific identification
			SyncRequest request = new SyncRequest()
			{
				UserId = Guid.Empty,
			};
			_client.Publish(Configs.RabbitMQ_PrepareSyncQueue, request);


			//TODO:: check if I can use a task to await the response outside the event
			//create here a task...start it in the Action...and also await it here
			_client.Subscribe<SyncRequestResponse>(Configs.RabbitMQ_SyncRequestQueue, n =>
			{
				if (n.Device.Equals(request.Device) && n.UserId.Equals(request.UserId))
				{
					//here I should unsubscribe from the SyncReqeustQueue 
					var notebooks = _unitOfWork.NotebooksRepository.GetNotebooksWithNotesByLastModifiedDateForNotes(n.LastSyncDate);
					Kernel.Get<ILog>().Info(Guid.Empty, "NotebooksWithNotesByLastModifiedDateForNotes received");


					_client.Publish(Configs.RabbitMQ_DataToBeSynchedQueue, new DataToBeSynched()
					{
						UserId = n.UserId,
						Notebooks = notebooks,
					});

					_client.Subscribe<SyncResult>(Configs.RabbitMQ_SyncResultQueue, syn =>
					{
						//TODO:: display message to the user that the sync was a succes/failure
						System.Diagnostics.Debug.WriteLine(syn.UserId + syn.Device + syn.Result);
					});
				}
			});
		}

		private void Connect()
		{
			if (!_client.Connected)
			{
				var headers = new Dictionary<string, string>()
				{
					{"login", Configs.RabbitMQ_Login },
					{"passcode", Configs.RabbitMQ_Password},
					{"host", Configs.RabbitMQ_Host },
					{ "accept-version", Configs.RabbitMQ_AcceptVersion },
				};
				_client.Connect(headers);
			}
		}
	}
}
