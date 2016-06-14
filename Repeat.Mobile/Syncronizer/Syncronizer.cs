using Android.App;
using Android.Content;
using Android.Widget;
using Newtonsoft.Json;
using Repeat.Mobile.PCL;
using Repeat.Mobile.PCL.APICallers.Interfaces;
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
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;
using Xamarin.StompClient;
using Xamarin.StompClient.Interfaces;

namespace Repeat.Mobile.Sync
{
	public class Syncronizer
	{

		private StompClient _client;
		private IUnitOfWork _unitOfWork;



		private Syncronizer()
		{
			_unitOfWork = Kernel.Get<IUnitOfWork>();

			_client = new StompClient(Configs.RabbitMQ_StompClient_Address);
		}

		public static Syncronizer CreateSyncher()
		{
			return new Syncronizer();
		}

		/// <summary>
		/// dbSyncStart - used to display UI  message/ do some action before starting local db syncronization with api
		/// dbSyncEnd - used to display UI  message/ do some action after ending local db syncronization with api
		/// </summary>
		/// <param name="dbSyncStart"></param>
		/// <param name="dbSyncEnd"></param>
		public void StartSynching(Action dbSyncStart, Action dbSyncEnd)
		{
			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken token = cts.Token;

			//temp for presentation:::if in 3 minutes the sync is not done close conn
			new Task(() =>
			{
				Task.Delay(TimeSpan.FromMinutes(3)).Wait();

				if (token.IsCancellationRequested)
				{
					Kernel.Get<ILog>().Info(Guid.Empty, "Monitoring will shut down task");
					token.ThrowIfCancellationRequested();
				}

				Kernel.Get<ILog>().Info(Guid.Empty, " Closing from task ");
				DisposeClient();

				dbSyncEnd();
			}, token).Start();

			//TODO:: maybe try a few times...and if it does not work display message to user
			Connect();

			//removes messages which due to the closing or lose of a connection remained on the queue
			RemoveExistingMessages();


			//here I should use the logged in user and device specific identification
			DTOs.SyncRequest request = new DTOs.SyncRequest()
			{
				UserId = Session.LoggedInUser.Id,
				UserToken = Session.LoggedInUser.Token,
			};

			_client.Subscribe<SyncRequestResponse>(Configs.RabbitMQ_SyncRequestQueue, n =>
			{
				if (n.Device.Equals(request.Device) && n.UserId.Equals(request.UserId))
				{

					var notebooks = _unitOfWork.NotebooksRepository.GetNotebooksWithNotesByLastModifiedDateOfNotes(n.LastSyncDate, Session.LoggedInUser.Id);

					_client.Publish(Configs.RabbitMQ_DataToBeSynchedQueue, new DataToBeSynched()
					{
						UserId = Session.LoggedInUser.Id,
						UserToken = Session.LoggedInUser.Token,
						Notebooks = notebooks,						
					});
				}
			}, true);

			_client.Subscribe<DTOs.SyncResult>(Configs.RabbitMQ_SyncResultQueue, syn =>
			{
				Kernel.Get<ILog>().Info(Guid.Empty, "Sync FINISHED");

				//temp solution
				Task.Factory.StartNew(() =>
				{
					Kernel.Get<ILog>().Info(Guid.Empty, "Dispose client");
					Task.Delay(10000).Wait();
					_client.Dispose();
					Kernel.Get<ILog>().Info(Guid.Empty, "Disposed");

					cts.Cancel();
				});

				GetSynchedDataToDB(dbSyncStart, dbSyncEnd);
			}, true);

			_client.Publish(Configs.RabbitMQ_PrepareSyncQueue, request);
		}

		private void DisposeClient()
		{
			lock (_obj)
			{
				_client.Dispose();
			}
		}
		private readonly object _obj = new object();


		private void RemoveExistingMessages()
		{
			_client.Subscribe<SyncRequestResponse>(Configs.RabbitMQ_SyncRequestQueue, n =>
			{
				Kernel.Get<ILog>().Info(Guid.Empty, "Dead msg intercepted");
			});

			_client.Subscribe<DTOs.SyncResult>(Configs.RabbitMQ_SyncResultQueue, syn =>
			{
				Kernel.Get<ILog>().Info(Guid.Empty, "Dead msg intercepted");
			});

			Thread.Sleep(5000);

			_client.Dispose();
			//_client = null;
			_client = new StompClient(Configs.RabbitMQ_StompClient_Address);

			Connect();
		}

		private async void GetSynchedDataToDB(Action dbSyncStart, Action dbSyncEnd)
		{

			var headers = new Dictionary<string, string>() { { "Authorization", Session.LoggedInUser.Token } };
			var apiNotebooks = await Kernel.Get<INotebookAPICaller>().GetList(string.Format(Configs.NotebooksAPI_Notebooks_Get, false, Session.LoggedInUser.Id), headers);

			var apiNotes = await Kernel.Get<INoteAPICaller>().GetList(string.Format(Configs.NotebooksAPI_Notes_Get, false), headers);

			dbSyncStart();

			//TODO:: here display msg to User...that his data is getting synched && also should stop all actions on UI

			//_unitOfWork.Dispose();
			//_unitOfWork = new UnitOfWork();
			_unitOfWork.NotesRepository.EraseAll(Session.LoggedInUser.Id);
			_unitOfWork.NotebooksRepository.EraseAll(Session.LoggedInUser.Id);

			try {
				foreach (var nb in apiNotebooks)
				{
					_unitOfWork.NotebooksRepository.Add(nb);

					foreach(var note in nb.Notes)
					{
						_unitOfWork.NotesRepository.Add(note);
					}
				}

				_unitOfWork.SaveChanges();
			}
			catch (Exception e)
			{

			}		

			//here end msg
			dbSyncEnd();
		}

		private void Connect()
		{
			var headers = new Dictionary<string, string>()
				{
					{"login", Configs.RabbitMQ_Login },
					{"passcode", Configs.RabbitMQ_Password},
					{"host", Configs.RabbitMQ_Host },
					{"accept-version", Configs.RabbitMQ_AcceptVersion },
					{"heart-beat", "50000,50000" }//I hardcoded 10000 inside the StompClient code
				};
			_client.Connect(headers,
			   () => Kernel.Get<ILog>().Info(Guid.Empty, "CONNECTED to WebSocket."));
		}
	}
}
