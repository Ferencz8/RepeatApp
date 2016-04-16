using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.Interfaces;
using System;

namespace Repeat.SyncronizerService
{
	public class RabbitMQ : IQueue
	{
		private IConnection _connection;
		private IModel _channel;
		private bool _disposed;
		private static readonly object _obj = new object();


		public IConnection Connection
		{
			get
			{
				lock (_obj)
				{
					if (_connection == null)
					{
						_connection = CreateMQConnection();
					}

					return _connection;
				}
			}
		}


		public RabbitMQ()
		{
			_connection = this.Connection;

			_channel = _connection.CreateModel();
		}

		public void SendMessage<T>(string queueName, T message)
		{
			lock (_obj)
			{
				DeclareQueue(queueName);

				var body = ObjectConverter.Object_To_JSON_To_ByteArray(message);

				_channel.BasicPublish(exchange: "",
									 routingKey: queueName,
									 basicProperties: null,
									 body: body);
			}
		}

		public void ProcessMessage<T>(string queueName, Action<IQueue, T> actionOnMessage)
			where T : class
		{
			lock (_obj)
			{
				DeclareQueue(queueName);

				var consumer = new EventingBasicConsumer(_channel);
				consumer.Received += (model, ea) =>
				{
					var body = ea.Body;
					T message = ObjectConverter.ByteArray_To_JSON_To_Object<T>(body);
					actionOnMessage(this, message);
				};
				_channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);
			}
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
			//TODO:: if no connection can be made...send an email ??
			while (true)
			{
				try
				{
					return factory.CreateConnection();
				}
				catch (BrokerUnreachableException unreacheable)
				{
					Log.Info(unreacheable.ToString());
				}
			}
		}



		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				// Free any other managed objects here.
				if (!_channel.IsClosed)
				{
					_channel.Close();
				}
				_channel.Dispose();
				if (_connection.IsOpen)
				{
					_connection.Close();
				}
				_connection.Dispose();
			}

			// Free any unmanaged objects here.
			_disposed = true;
		}
	}
}
