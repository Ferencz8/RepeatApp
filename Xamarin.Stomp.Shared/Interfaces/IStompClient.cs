using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.StompClient.Interfaces
{
	public interface IStompClient
	{
		void Connect(Dictionary<string, string> authenticationHeaders = null);

		bool Connected { get; }

		void Publish(string queueName, object message);

		void Subscribe<T>(string queueName, Action<T> eventOnMessageReceived)
			where T : class;

		void Disconnect();
	}
}
