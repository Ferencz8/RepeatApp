using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.StompClient.Interfaces
{
	public interface IStompClient : IDisposable
	{
		void Connect(
			Dictionary<string, string> authenticationHeaders = null, 
			Action onConnectedCallBack = null,
			Action<object, ErrorEventArgs> onErrorCallBack = null);
		
		void Publish(string queueName, object message);

		void Subscribe<T>(string queueName, Action<T> eventOnMessageReceived, bool useAckowledgment = false)
			where T : class;

		void Disconnect();
	}
}
