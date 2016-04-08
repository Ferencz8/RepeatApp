using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.StompClient.Interfaces
{
	public interface IMessage
	{
		string Command { get; }

		Dictionary<string, string> Headers { get; }

		object Body { get; }
	}
}
