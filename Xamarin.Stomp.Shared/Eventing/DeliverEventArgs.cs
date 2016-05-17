using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.StompClient.Interfaces;

namespace Xamarin.Stomp.Shared.Eventing
{
    public class DeliverEventArgs : EventArgs
    {
		public DeliverEventArgs(IMessage message)
		{
			Message = message;
		}

		public IMessage Message { get; set; }
	}
}
