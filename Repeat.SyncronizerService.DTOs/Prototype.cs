using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DTOs
{
	public class Prototype<T>
		where T : class, new()
	{
		private JsonSerializerSettings _settings = new JsonSerializerSettings()
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};


		public T Clone()
		{
			//create a deep copy
			return CreateDeepObjectClone<T>(this);
		}
		
		protected Y Clone<Y>()
			where Y : class, new()
		{
			//create a deep copy	
			return CreateDeepObjectClone<Y>(this);
		}

		private N CreateDeepObjectClone<N>(object obj)
		{
			//create a deep copy
			string serializedObj = JsonConvert.SerializeObject(obj, _settings);

			return JsonConvert.DeserializeObject<N>(serializedObj);
		}
	}
}
