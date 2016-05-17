using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.StompClient;

namespace Xamarin.Stomp.Shared
{
	public class ACKMessage : Message
	{

		public ACKMessage(string messageId)
			: base("ACK",
				 new Dictionary<string, string>()
				 {
					  {"message-id", messageId }
				 },
				 null)
		{
		}
	}
}
