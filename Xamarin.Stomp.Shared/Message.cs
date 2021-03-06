﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.StompClient.Interfaces;

namespace Xamarin.StompClient
{
	public class Message : IMessage
	{
		private readonly string _command;
		private readonly Dictionary<string, string> _headers;
		private readonly object _body;

		public Message(string command, Dictionary<string, string> headers, object body)
		{
			_command = command;
			_headers = headers;
			_body = body;
		}

		public string Command
		{
			get { return _command; }
		}

		public Dictionary<string, string> Headers
		{
			get { return _headers; }
			protected set
			{

			}
		}

		public object Body
		{
			get { return _body; }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Command + "\n");
			
			foreach(var header in Headers)
			{
				sb.Append(header.Key + ":" + header.Value);
			}

			sb.Append("\n" + JsonConvert.SerializeObject(Body));

			return sb.ToString();
		}
	}
}
