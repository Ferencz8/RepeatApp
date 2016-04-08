using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Stomp.Shared;
using Xamarin.StompClient.Interfaces;

namespace Xamarin.StompClient
{
	public class MessageSerializer
	{
		public string Serialize(IMessage messageToBeSerialized)
		{
			string response = string.Empty;
			using (StreamWriter _streamWriter = new StreamWriter(new MemoryStream()))
			{   // Command
				_streamWriter.WriteLine(messageToBeSerialized.Command);

				//Headers
				byte[] bodyBuffer = CovertObjToByteArray(messageToBeSerialized.Body);
				// Content-length header.
				_streamWriter.WriteLine("content-length:{0}", bodyBuffer.Length);

				foreach (var header in messageToBeSerialized.Headers)
				{
					_streamWriter.WriteLine("{0}:{1}", header.Key, header.Value);
				}

				_streamWriter.WriteLine();
				_streamWriter.Flush();

				// Body 
				_streamWriter.BaseStream.Write(bodyBuffer, 0, bodyBuffer.Length);

				// Null
				_streamWriter.WriteLine((char)0);

				_streamWriter.Flush();

				_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
				using (StreamReader reader = new StreamReader(_streamWriter.BaseStream))
				{
					response = reader.ReadToEnd();
				}
			}
			return response;
		}

		public IMessage Deserialize(string serializedMessage)
		{
			if (!string.IsNullOrEmpty(serializedMessage))
			{
				string[] parts = serializedMessage.Split('\n').Where(n => !string.IsNullOrEmpty(n)).ToArray();
				string command = parts[0];
				MessageBuilder messageBuilder = new MessageBuilder(command);
				int length = parts.Length - 1;
				string body = string.Empty;
				if (command == MessageCommand.Message)
				{
					body = parts.Last();
				}
				for (int i = 1; i < length; i++)
				{
					string[] header = parts[i].Split(':');
					messageBuilder.Header(header[0], header[1]);
				}
				return messageBuilder.WithBody(body);
			}

			return null;
		}
		
		private byte[] CovertObjToByteArray(object obj)
		{
			string objSerialized = obj != null ? JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			}) : string.Empty;

			return Encoding.UTF8.GetBytes(objSerialized);
		}
	}
}
