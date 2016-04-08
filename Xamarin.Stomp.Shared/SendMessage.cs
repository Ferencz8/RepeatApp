using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.StompClient.Interfaces;

namespace Xamarin.StompClient
{
	public class SendMessage : Message
	{

		public SendMessage(string destination, object body)
			:base(
				 "SEND", 
				 new Dictionary<string, string>()
				 {
				 	{ "destination", destination }
				 }, 
				 body)
		{

		}
	}
}
