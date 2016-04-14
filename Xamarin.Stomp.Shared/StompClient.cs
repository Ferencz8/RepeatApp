using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using WebSocket4Net;
using Xamarin.Stomp.Shared;
using Xamarin.StompClient.Interfaces;
using Newtonsoft.Json;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.Logging;
using System.Threading;
using Xamarin.Stomp.Shared.Eventing;
using System.Runtime.Remoting.Contexts;

namespace Xamarin.StompClient
{
	//used to create a lock for every method, property
	//[Synchronization]
	public class StompClient : IStompClient
	{
		static readonly object _obj = new object();
		bool _disposed = false;

		WebSocket _webSocket;
		IMessage _authenticationMessage;
		MessageSerializer _messageSerializer;
		List<EventingMessageConsumer> _subscribedConsumers;
		Action _onConnectedCallback;

		public bool Connected
		{
			get
			{
				lock (_obj)
				{
					return _webSocket.State == WebSocketState.Open ? true : false;
				}
			}
		}

		public StompClient(string address)
		{
			_webSocket = new WebSocket(address);

			_messageSerializer = new MessageSerializer();


			_webSocket.Opened += new EventHandler(Websocket_Opened);

			_webSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(Websocket_MessageReceived);
		}


		public async Task<bool> Connect(
			int timeoutSeconds = 60,
			Dictionary<string, string> authenticationHeaders = null,
			Action onConnectedCallBack = null,
			Action<object, ErrorEventArgs> onErrorCallBack = null)
		{

			HandleConnectionParameters(authenticationHeaders, onConnectedCallBack, onErrorCallBack);


			//if the connection is lost all the subscribtion messages need to be sent again
			_subscribedConsumers = new List<EventingMessageConsumer>();


			_webSocket.Open();

			//wait until Connected becomes true or the max waiting time elapses
			await Task.Run(delegate
			{
				while (!Connected && timeoutSeconds >= 0)
				{
					Task.Delay(TimeSpan.FromSeconds(1)).Wait();

					timeoutSeconds--;
				}
			});

			return Connected;
		}


		public bool Publish(string queueName, object messageBody)
		{
			if (!Connected)
			{
				return false;
			}

			var sendMessage = new SendMessage(queueName, messageBody);
			string msgToBeSnet = _messageSerializer.Serialize(sendMessage);

			_webSocket.Send(msgToBeSnet);

			Kernel.Get<ILog>().Info(Guid.Empty, "PUBLISHED on " + queueName);

			return true;
		}

		public bool Subscribe<T>(string queueName, Action<T> eventOnMessageReceived, bool useAckowledgment = false) where T : class
		{
			if (!Connected)
			{
				return false;
			}

			if (_subscribedConsumers.Exists(n => n.QueueName.Equals(queueName)))
			{
				//remove existing event handler
				EventingMessageConsumer consumerToBeRemoved = _subscribedConsumers.FirstOrDefault(n => n.QueueName.Equals(queueName));

				_subscribedConsumers.Remove(consumerToBeRemoved);
			}
			else
			{
				//subscribe message should be sent only once
				SubscribeMessage subscribeMessage = new SubscribeMessage(queueName, useAckowledgment);
				string serializedMessage = _messageSerializer.Serialize(subscribeMessage);

				_webSocket.Send(serializedMessage);
			}


			EventingMessageConsumer consumer = new EventingMessageConsumer("/queue/" + queueName, useAckowledgment);

			consumer.OnMessageReceived += (obj, args) =>
			{
				//Kernel.Get<ILog>().Info(Guid.Empty, "HANDLER EXECUTED!!! " + queueName);

				T objConverted = JsonConvert.DeserializeObject<T>(args.Message.Body.ToString());
				
				eventOnMessageReceived(objConverted);

				if (consumer.UseACKnowledgement)
				{
					SendACKMessage(args.Message.Headers["message-id"]);
				}
			};

			_subscribedConsumers.Add(consumer);

			//Kernel.Get<ILog>().Info(Guid.Empty, "SUBSCRIBED on " + queueName);

			return true;
		}

		/// <summary>
		/// This Heart Beat task will use send a heart beat every 10 seconds. 
		/// </summary>
		/// <param name="serverHeartBeat"></param>
		private void StartHeartBeatTask()
		{

			Task.Factory.StartNew(() =>
			{
				while (Connected)
				{
					Task.Delay(TimeSpan.FromSeconds(7)).Wait();

					_webSocket.Send("\n");
				}
			});
		}

		private void SendACKMessage(string messageId)
		{
			string msgToBeSnet = _messageSerializer.Serialize(new ACKMessage(messageId));

			_webSocket.Send(msgToBeSnet);
		}

		private void HandleConnectionParameters(Dictionary<string, string> authenticationHeaders, Action onConnectedCallBack, Action<object, ErrorEventArgs> onErrorCallBack)
		{

			if (authenticationHeaders != null)
			{
				_authenticationMessage = new Message(MessageCommand.Connect, authenticationHeaders, null);
			}

			if (onConnectedCallBack != null)
			{
				_onConnectedCallback = onConnectedCallBack;
			}

			if (onErrorCallBack != null)
			{
				_webSocket.Error += new EventHandler<ErrorEventArgs>(onErrorCallBack);
			}
			else
			{
				_webSocket.Error += new EventHandler<ErrorEventArgs>(WebSocket_ErrorHandler);
			}
		}


		private void Websocket_Opened(object sender, EventArgs e)
		{
			string msgToSend = _messageSerializer.Serialize(_authenticationMessage);
			_webSocket.Send(msgToSend);
		}

		private void Websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			IMessage messageReceived = _messageSerializer.Deserialize(e.Message);
			if (messageReceived == null)
				return;
			switch (messageReceived.Command)
			{
				case MessageCommand.Connected:
					{
						StartHeartBeatTask();

						if (_onConnectedCallback != null)
						{
							_onConnectedCallback();
						}
					}
					break;
				case MessageCommand.Message:
					{
						Kernel.Get<ILog>().Info(Guid.Empty, "RECEIVED Message: on " + messageReceived.Headers["destination"]);

						var consumer = _subscribedConsumers.FirstOrDefault(n => n.QueueName.Equals(messageReceived.Headers["destination"]));
						if (consumer != null)
						{
							consumer.FireUpEventHanlder(sender, new DeliverEventArgs(messageReceived));
						}
					}
					break;
				default: break;
			}
		}

		private void WebSocket_ErrorHandler(object sender, ErrorEventArgs e)
		{

		}


		public void Disconnect()
		{
			string serializedMsg = _messageSerializer.Serialize(new DisconnectMessage());
			_webSocket.Send(serializedMsg);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				Disconnect();
			}

			// Free any unmanaged objects here.
			//
			_disposed = true;
		}
	}
}
