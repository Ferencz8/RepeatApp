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
		Task<bool> Connect(
			int timeoutSeconds = 60, 
			Dictionary<string, string> authenticationHeaders = null, 
			Action onConnectedCallBack = null,
			Action<object, ErrorEventArgs> onErrorCallBack = null);

		bool Connected { get; }

		bool Publish(string queueName, object message);

		bool Subscribe<T>(string queueName, Action<T> eventOnMessageReceived, bool useAckowledgment = false)
			where T : class;

		void Disconnect();
	}
}
