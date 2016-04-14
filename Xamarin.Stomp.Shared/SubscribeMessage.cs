using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.StompClient
{
	public class SubscribeMessage : Message
	{
		/// <summary>
		/// To the destination parameter /queue/ will automatically be appended at the beggining.
		/// </summary>
		/// <param name="destination"></param>

		public SubscribeMessage(string destination, bool useAcknowledgements = false)
			: base("SUBSCRIBE",
				 new Dictionary<string, string>()
				 {
					  {"destination", "/queue/" + destination }
				 },
				 null)
		{
			if (useAcknowledgements)
			{
				Headers.Add("ack", "client");
			}
		}
	}
}
