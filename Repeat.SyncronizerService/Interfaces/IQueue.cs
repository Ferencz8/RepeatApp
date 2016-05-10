using RabbitMQ.Client;
using Repeat.SyncronizerService.Strategies;
using System;

namespace Repeat.SyncronizerService.Interfaces
{
	public interface IQueue : IDisposable
	{
		IConnection Connection { get; }

		void SendMessage<T>(string queueName, T message);

		void ProcessMessage<T>(string queueName, IQueueProcessor<T> processor)
			where T : class;
	}
}
