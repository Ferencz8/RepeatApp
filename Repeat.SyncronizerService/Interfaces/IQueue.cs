using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.Interfaces
{
	public interface IQueue : IDisposable
	{
		void SendMessage<T>(string queueName, T message);

		void ProcessMessage<T>(string queueName, Action<T> actionOnMessage) 
			where T : class;
	}
}
