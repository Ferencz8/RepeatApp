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
		//https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
		// Convert an object to a byte array
		public static byte[] ObjectToByteArray(Object obj)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		// Convert a byte array to an Object
		public static T ByteArrayToObject<T>(byte[] arrBytes)
			where T : class
		{
			using (var memStream = new MemoryStream())
			{
				var binForm = new BinaryFormatter();
				memStream.Write(arrBytes, 0, arrBytes.Length);
				memStream.Seek(0, SeekOrigin.Begin);
				var obj = binForm.Deserialize(memStream) as T;				
				return obj;
			}
		}
	}
}
