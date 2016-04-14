using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.Common
{
	public static class ObjectConverter
	{

		private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		public static string ToJSON<T>(T obj)
			where T : class
		{
			return JsonConvert.SerializeObject(obj, _settings);
		}

		public static T ToObject<T>(string jsonStringOfObject)
			where T : class
		{
			return JsonConvert.DeserializeObject<T>(jsonStringOfObject, _settings);
		}
	}
}
