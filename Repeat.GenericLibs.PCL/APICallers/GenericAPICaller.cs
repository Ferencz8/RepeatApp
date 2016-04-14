using Newtonsoft.Json;
using Repeat.GenericLibs.PCL.APICallers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.GenericLibs.PCL.APICallers
{
	public class GenericAPICaller<T> : IGenericAPICaller<T>
	{
		protected string _apiURL;
		protected JsonSerializerSettings _settings = new JsonSerializerSettings()
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		public GenericAPICaller(string apiURL)
		{
			_apiURL = apiURL;
		}

		public async void Add(string apiRoute, T obj)
		{
			try
			{
				using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
				{
					var stringContent = new StringContent(JsonConvert.SerializeObject(obj, _settings), Encoding.UTF8, "application/json");
					var response = await client.PostAsync(apiRoute, stringContent);
					if (response != null)
					{
						//TODO:: do smth with the response
					}
				}
			}
			catch (Exception e)
			{
				//TODO LOG
			}
		}

		public async void Delete(string apiRoute, object id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<T>> GetList(string apiRoute)
		{
			List<T> elements = new List<T>();
			try
			{
				using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
				{
					var response = await client.GetAsync(apiRoute);
					if (response != null)
					{
						string str = await response.Content.ReadAsStringAsync();
						elements = JsonConvert.DeserializeObject<List<T>>(str, _settings);
					}
				}
			}
			catch (Exception e)
			{
				//TODO LOG
			}
			return elements;
		}

		public async Task<T> Get(string apiRoute)
		{
			T element = default(T);
			try
			{
				using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
				{
					var response = await client.GetAsync(apiRoute);
					if (response != null)
					{
						string str = await response.Content.ReadAsStringAsync();
						element = JsonConvert.DeserializeObject<T>(str, _settings);
					}
				}
			}
			catch (Exception e)
			{
				//TODO LOG
			}
			return element;
		}

		public async void Update(string apiRoute, T obj)
		{
			try
			{
				using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
				{
					var stringContent = new StringContent(JsonConvert.SerializeObject(obj, _settings), Encoding.UTF8, "application/json");
					var response = await client.PutAsync(apiRoute, stringContent);
					if (response != null)
					{
						//TODO:: do smth with the response
					}
				}
			}
			catch (Exception e)
			{
				//TODO LOG
			}
		}
	}
}
