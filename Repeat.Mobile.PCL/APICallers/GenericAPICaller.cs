//using Newtonsoft.Json;
//using Repeat.Common;
//using Repeat.Mobile.PCL.APICallers.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Repeat.Mobile.PCL.APICallers
//{
//	public class GenericAPICaller<T> : IGenericAPICaller<T>
//	{
//		public async void Add(string apiRoute, T obj)
//		{
//			throw new NotImplementedException();
//		}

//		public async void Delete(string apiRoute, object id)
//		{
//			throw new NotImplementedException();
//		}

//		public async Task<List<T>> Get(string apiRoute)
//		{
//			List<T> elements = new List<T>();
//			try
//			{
//				var client = HttpClientExtensions.GetAPIClient();
//				var response = await client.GetAsync(apiRoute);
//				if (response != null)
//				{
//					string str = await response.Content.ReadAsStringAsync();
//					elements = JsonConvert.DeserializeObject<List<T>>(str);
//				}
//			}
//			catch (Exception e)
//			{
//				//TODO LOG
//			}
//			return elements;
//		}

//		public async Task<T> GetById(string apiRoute)
//		{
//			throw new NotImplementedException();
//		}

//		public async void Update(string apiRoute, T obj)
//		{
//			throw new NotImplementedException();
//		}
//	}
//}
