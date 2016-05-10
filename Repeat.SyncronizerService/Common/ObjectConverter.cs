using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.Common
{
	public class ObjectConverter
	{
		
		private static JsonSerializerSettings _settings = new JsonSerializerSettings()
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		//https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
		// Convert an object to a byte array
		public static byte[] Object_To_JSON_To_ByteArray(object obj)
		{
			var messageAsJson = JsonConvert.SerializeObject(obj, _settings);

			return Encoding.UTF8.GetBytes(messageAsJson);
		}

		// Convert a byte array to an Object
		public static T ByteArray_To_JSON_To_Object<T>(byte[] arrBytes)
			where T : class
		{

			var messageAsJson = Encoding.UTF8.GetString(arrBytes);

			return JsonConvert.DeserializeObject<T>(messageAsJson, _settings);
		}
	}
}
