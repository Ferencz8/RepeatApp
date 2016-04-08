using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.StompClient.Interfaces
{
	public interface IOutgoingMessage
	{
		byte[] Body { get; }

		IEnumerable<KeyValuePair<string, string>> Headers { get; }
	}
}
