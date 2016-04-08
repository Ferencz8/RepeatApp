using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.StompClient.Interfaces;

namespace Xamarin.StompClient
{
	public class MessageBuilder
	{
		private readonly string _command;

		private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

		public MessageBuilder(string command)
		{
			_command = command;
		}

		public MessageBuilder Header(string key, string value)
		{
			_headers.Add(key, value);
			return this;
		}

		public IMessage WithBody(object body)
		{
			return new Message(_command, _headers, body);
		}

		public IMessage WithoutBody()
		{
			return new Message(_command, _headers, null);
		}
	}
}
