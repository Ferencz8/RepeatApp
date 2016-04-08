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

namespace Xamarin.StompClient
{
	public class StompClient : IStompClient
	{
		//TODO:: check the heart-beat header meaning??
		WebSocket _webSocket;
		IMessage _authenticationMessage;
		MessageSerializer _messageSerializer = new MessageSerializer();
		Dictionary<string, KeyValuePair<Type, Delegate>> _subscribedHandlers = new Dictionary<string, KeyValuePair<Type, Delegate>>();
		Queue<string> _messagesToBeSent = new Queue<string>();

		public bool Connected
		{
			get
			{
				return _webSocket.State == WebSocketState.Open ? true : false;
			}
		}

		public StompClient(string address)
		{
			_webSocket = new WebSocket(address);

			_webSocket.Opened += new EventHandler(Websocket_Opened);

			_webSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(Websocket_MessageReceived);

			_webSocket.Error += new EventHandler<ErrorEventArgs>(WebSocket_ErrorHandler);
		}

		private void WebSocket_ErrorHandler(object sender, ErrorEventArgs e)
		{
			//TODO:: implement error handler for websocket
			//TODO::implement global error handler
			System.Diagnostics.Debug.WriteLine(e.Exception.ToString());
		}

		public void Connect(Dictionary<string, string> authenticationHeaders = null)
		{
			if (authenticationHeaders != null)
			{
				_authenticationMessage = new Message(MessageCommand.Connect, authenticationHeaders, null);
			}

			_webSocket.Open();
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
						foreach (var ms in _messagesToBeSent)
						{
							_webSocket.Send(ms);
						}
					}
					break;
				case MessageCommand.Message:
					{
						var destinationHeader = messageReceived.Headers["destination"];
						//TODO:: here convert according to  _subscribedHandlers[destinationHeader].Key the object 
						Type type = _subscribedHandlers[destinationHeader].Key;
						var deserializedObj = JsonConvert.DeserializeObject(messageReceived.Body.ToString(), type);
						_subscribedHandlers[destinationHeader].Value.DynamicInvoke(deserializedObj);
					}
					break;
				default: break;
			}
		}

		public void Disconnect()
		{
			throw new NotImplementedException();
		}

		public void Publish(string queueName, object messageBody)
		{
			//TODO::this messages first should be added to a queue and only if connection is CONNECTED then they should be published

			var sendMessage = new SendMessage("/queue/" + queueName, messageBody);
			string msgToBeSnet = _messageSerializer.Serialize(sendMessage);
			if (_webSocket.State == WebSocketState.Open)
			{
				_webSocket.Send(msgToBeSnet);
			}
			else
			{
				_messagesToBeSent.Enqueue(msgToBeSnet);
			}
		}

		public void Subscribe<T>(string queueName, Action<T> eventOnMessageReceived) where T : class
		{
			string key = "/queue/" + queueName;
			if (!_subscribedHandlers.ContainsKey(key))
			{
				_subscribedHandlers.Add(key, new KeyValuePair<Type, Delegate>(typeof(T), eventOnMessageReceived));

				var subscribeMessage = new SubscribeMessage("/queue/" + queueName);
				string serializedMessage = _messageSerializer.Serialize(subscribeMessage);
				if (_webSocket.State == WebSocketState.Open)
				{
					_webSocket.Send(serializedMessage);
				}
				else
				{
					_messagesToBeSent.Enqueue(serializedMessage);
				}
			}
		}
	}
}
