using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Stomp.Shared.Eventing
{
    public class EventingMessageConsumer
    {

		public event EventHandler OnMessageReceived;

		public delegate void EventHandler(object sender, DeliverEventArgs args);


		public string QueueName { get; set; }

		public bool UseACKnowledgement { get; set; }

		public EventingMessageConsumer(string queueName, bool useACK = false)
		{
			QueueName = queueName;
			UseACKnowledgement = useACK;
		}

		public void FireUpEventHanlder(object sender, DeliverEventArgs args)
		{
			OnMessageReceived(sender, args);
		}
	}
}
