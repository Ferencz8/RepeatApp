using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.StompClient
{
	public class SubscribeMessage : Message
	{

		public SubscribeMessage(string destination)
			: base("SUBSCRIBE",
				 new Dictionary<string, string>()
				 {
					  {"destination", destination }
				 },
				 null)
		{

		}
	}
}
