using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Repeat.NotebooksAPI
{
    public class ObjectSerializer
    {

		public static string SerializeToJSON(object objToSerialize)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			return serializer.Serialize(objToSerialize);
		}

		public static T DeserializeFronJSON<T>(string jsonToDeserialize)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			return serializer.Deserialize<T>(jsonToDeserialize);
		} 
	}
}
