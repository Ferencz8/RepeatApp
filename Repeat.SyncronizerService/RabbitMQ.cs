using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService
{
	public class RabbitMQ : IQueue
	{
		private IConnection _connection;
		private IModel _channel;


		public IConnection Connection
		{
			get
			{
				if(_connection == null)
				{
					_connection = CreateMQConnection();
				}

				return _connection;
			}
		}
		

		public RabbitMQ()
		{
			_connection = this.Connection;

			_channel = _connection.CreateModel();
		}

		public void SendMessage<T>(string queueName, T message)
		{
			DeclareQueue(queueName);

			var body = ObjectConverter.ObjectToByteArray(message);

			_channel.BasicPublish(exchange: "",
								 routingKey: queueName,
								 basicProperties: null,
								 body: body);
		}

		public void ProcessMessage<T>(string queueName, Action<T> actionOnMessage)
			where T : class
		{
			DeclareQueue(queueName);

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body;
				T message = ObjectConverter.JSONToObject<T>(body);
				actionOnMessage(message);
			};
			_channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}



		private void DeclareQueue(string queueName)
		{
			_channel.QueueDeclare(queue: queueName,
				durable: true,
				exclusive: false,
				autoDelete: false,
				arguments: null);
		}

		private IConnection CreateMQConnection()
		{
			ConnectionFactory factory = new ConnectionFactory
			{
				UserName = Config.RabbitMQUsername,
				Password = Config.RabbitMQPassword,
				HostName = Config.RabbitMQHostName,
				Port = Int32.Parse(Config.RabbitMQPort),
				VirtualHost = Config.RabbitMQVirtualHost,
			};

			return factory.CreateConnection();
		}
	}
}
