using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.StompClient;

namespace Xamarin.Stomp.Shared
{
	public class DisconnectMessage : Message
	{
		public DisconnectMessage()
			: base("DISCONNECT", new Dictionary<string, string>(), null)
		{
		}
	}
}
