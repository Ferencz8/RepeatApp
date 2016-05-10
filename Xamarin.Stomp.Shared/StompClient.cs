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
using System.Collections.Concurrent;

namespace Xamarin.StompClient
{
	public class StompClient : IStompClient
	{

		static readonly object _obj = new object();
		bool _disposed = false;

		bool _disconnectRequest;
		WebSocket _webSocket;
		IMessage _authenticationMessage;
		MessageSerializer _messageSerializer;
		List<EventingMessageConsumer> _subscribedConsumers;
		Action _onConnectedCallback;
		Queue<string> _delayedMessagesToBeSent;
		Dictionary<string, string> _subscribedMessages = new Dictionary<string, string>();
		List<CancellationTokenSource> _threadTokens = new List<CancellationTokenSource>();


		private bool Connected
		{
			get
			{
				return _webSocket.State == WebSocketState.Open || _webSocket.State == WebSocketState.Connecting ? true : false;
			}
		}

		public StompClient(string address)
		{
			_delayedMessagesToBeSent = new Queue<string>();

			_webSocket = new WebSocket(address);

			_messageSerializer = new MessageSerializer();

			_subscribedConsumers = new List<EventingMessageConsumer>();

			_webSocket.Opened += new EventHandler(Websocket_Opened);

			_webSocket.Error += WebSocket_ErrorHandler;

			_webSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(Websocket_MessageReceived);

			_webSocket.Closed += _webSocket_Closed;
		}


		public void Connect(
			Dictionary<string, string> authenticationHeaders = null,
			Action onConnectedCallBack = null,
			Action<object, ErrorEventArgs> onErrorCallBack = null)
		{
			if (!Connected)
			{
				HandleConnectionParameters(authenticationHeaders, onConnectedCallBack, onErrorCallBack);

				_webSocket.Open();

				_disconnectRequest = false;
			}
		}


		public void Publish(string queueName, object messageBody)
		{

			var sendMessage = new SendMessage(queueName, messageBody);
			string msgToBeSnet = _messageSerializer.Serialize(sendMessage);

			RegisterMessageToBeSentLater(msgToBeSnet);
		}

		public void Subscribe<T>(string queueName, Action<T> eventOnMessageReceived, bool useAckowledgment = false) where T : class
		{
			//TODO:: list of _subscribedConsumers and _subscribedMessages should be one collection
			if (_subscribedConsumers.Exists(n => n.QueueName.Equals("/queue/" + queueName)))
			{
				//remove existing event handler
				EventingMessageConsumer consumerToBeRemoved = _subscribedConsumers.FirstOrDefault(n => n.QueueName.Equals("/queue/" + queueName));

				_subscribedConsumers.Remove(consumerToBeRemoved);//remove the subscriber event handler
				_subscribedMessages.Remove("/queue/" + queueName);//and message
			}

			//subscribe message should be sent only once
			SubscribeMessage subscribeMessage = new SubscribeMessage(queueName, useAckowledgment);
			string serializedMessage = _messageSerializer.Serialize(subscribeMessage);

			_subscribedMessages.Add("/queue/" + queueName, serializedMessage);
			RegisterMessageToBeSentLater(serializedMessage);



			EventingMessageConsumer consumer = new EventingMessageConsumer("/queue/" + queueName, useAckowledgment);

			consumer.OnMessageReceived += (obj, args) =>
			{
				T objConverted = JsonConvert.DeserializeObject<T>(args.Message.Body.ToString());

				eventOnMessageReceived(objConverted);

				if (consumer.UseACKnowledgement)
				{
					SendACKMessage(args.Message.Headers["message-id"]);
				}
			};

			_subscribedConsumers.Add(consumer);
		}
		/// <summary>
		/// This Heart Beat task will use send a heart beat every 10 seconds. 
		/// </summary>
		/// <param name="serverHeartBeat"></param>
		private void StartHeartBeatTask()
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancelationToken = cancellationTokenSource.Token;

			Kernel.Get<ILog>().Info(Guid.Empty, "Starting Heart Beat task");
			Task.Factory.StartNew(() =>
			{
				while (true)
				{

					Task.Delay(10000).Wait();
					if (cancelationToken.IsCancellationRequested)
					{
						Kernel.Get<ILog>().Info(Guid.Empty, "Cancel task heart beat ");

						cancelationToken.ThrowIfCancellationRequested();
					}

					SendMessage("\n");
					//Kernel.Get<ILog>().Info(Guid.Empty, "heart beat sent");
				}
			}, cancelationToken);

			_threadTokens.Add(cancellationTokenSource);
		}

		private void Log(string message, Exception e = null)
		{
			if (e == null)
			{
				Kernel.Get<ILog>().Info(Guid.Empty, message);
			}
			else
			{
				Kernel.Get<ILog>().Exception(Guid.Empty, e, message);
			}
		}

		private void SendACKMessage(string messageId)
		{
			string msgToBeSnet = _messageSerializer.Serialize(new ACKMessage(messageId));

			RegisterMessageToBeSentLater(msgToBeSnet);
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
				_webSocket.Error -= WebSocket_ErrorHandler;
				_webSocket.Error += new EventHandler<ErrorEventArgs>(onErrorCallBack);
			}
		}

		private void SendMessage(string msg)
		{
			lock (_obj)
			{
				if (!Connected)
				{
					Log("Lost conn while sendind msg");
				}
				_webSocket.Send(msg);

				if (!string.IsNullOrWhiteSpace(msg) && !msg.Equals("\n"))
				{
					Kernel.Get<ILog>().Info(Guid.Empty, "SENT message with delayed queue" + msg.Substring(0, msg.Length > 60 ? 60 : msg.Length));
				}
			}
		}

		private void Websocket_Opened(object sender, EventArgs e)
		{
			string msgToSend = _messageSerializer.Serialize(_authenticationMessage);
			SendMessage(msgToSend);
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

						StartTask_For_SendingMessages();
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


		private void _webSocket_Closed(object sender, EventArgs e)
		{
			if (!_disconnectRequest)
			{
				Kernel.Get<ILog>().Info(Guid.Empty, "Connection Lost");

				if (!Connected)
				{
					Kernel.Get<ILog>().Info(Guid.Empty, "Reconnecting");

					Connect();


					Thread.Sleep(2000);

					//subscribe again
					foreach (var msg in _subscribedMessages)
					{
						RegisterMessageToBeSentLater(msg.Value);
					}
				}
			}
		}

		private void WebSocket_ErrorHandler(object sender, ErrorEventArgs e)
		{
			Kernel.Get<ILog>().Exception(Guid.Empty, e.Exception, "Error handler");
		}


		public void Disconnect()
		{
			if (Connected)
			{
				_disconnectRequest = true;
				string serializedMsg = _messageSerializer.Serialize(new DisconnectMessage());
				SendMessage(serializedMsg);
			}
		}

		private void RegisterMessageToBeSentLater(string msg)
		{
			lock (_obj2)
			{
				_delayedMessagesToBeSent.Enqueue(msg);
			}
		}

		private readonly object _obj2 = new object();

		private void StartTask_For_SendingMessages()
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancelationToken = cancellationTokenSource.Token;

			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					Task.Delay(1000).Wait();

					lock (_obj2)
					{
						if (_delayedMessagesToBeSent.Count > 0)
						{
							string msg = _delayedMessagesToBeSent.Peek();

							SendMessage(msg);
						}

						//stop task
						if (cancelationToken.IsCancellationRequested)
						{
							Kernel.Get<ILog>().Info(Guid.Empty, "Canceling task sending messages");
							cancelationToken.ThrowIfCancellationRequested();
						}
						else//then the message got sent succesfully
						{
							if (_delayedMessagesToBeSent.Count > 0)
							{
								string msg = _delayedMessagesToBeSent.Dequeue();
							}
						}
					}
				}
			}, cancelationToken);

			_threadTokens.Add(cancellationTokenSource);
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
				_threadTokens.ForEach(n => n.Cancel());//cancel the heart beat and delayed message sender tasks
				_threadTokens.Clear();

				Disconnect();
			}

			// Free any unmanaged objects here.
			//
			_disposed = true;
		}
	}
}
