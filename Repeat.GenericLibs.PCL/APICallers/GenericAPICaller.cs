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

		public async void Add(string apiRoute, T obj, Dictionary<string, string> headers = null)
		{
			for (int i = 0; i < 5; i++)
			{
				try
				{
					using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
					{
						client.AddHeaders(headers);
						var stringContent = new StringContent(JsonConvert.SerializeObject(obj, _settings), Encoding.UTF8, "application/json");
						var response = await client.PostAsync(apiRoute, stringContent);
						if (response != null)
						{
							//do smth with the response
						}

						break;
					}
				}
				catch (Exception e)
				{
					continue;
				}
			}
		}

		public async Task<List<T>> GetList(string apiRoute, Dictionary<string, string> headers = null)
		{
			List<T> elements = new List<T>();
			for (int i = 0; i < 5; i++)
			{
				try
				{
					using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
					{
						client.AddHeaders(headers);
						var response = await client.GetAsync(apiRoute);
						if (response != null)
						{
							string str = await response.Content.ReadAsStringAsync();
							elements = JsonConvert.DeserializeObject<List<T>>(str, _settings);
						}
					}
					break;
				}
				catch (Exception e)
				{
					continue;
				}
			}
			return elements;
		}

		public async Task<T> Get(string apiRoute, Dictionary<string, string> headers = null)
		{
			T element = default(T);
			for (int i = 0; i < 5; i++)
			{
				try
				{
					using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
					{
						client.AddHeaders(headers);
						var response = await client.GetAsync(apiRoute);
						if (response != null)
						{
							string str = await response.Content.ReadAsStringAsync();
							element = JsonConvert.DeserializeObject<T>(str, _settings);
						}
					}
					break;
				}
				catch (Exception e)
				{
					continue;
				}
			}
			return element;
		}

		public async void Update(string apiRoute, T obj, Dictionary<string, string> headers = null)
		{
			for (int i = 0; i < 5; i++)
			{
				try
				{
					using (var client = HttpClientExtensions.GetAPIClient(_apiURL))
					{
						client.AddHeaders(headers);
						var stringContent = new StringContent(JsonConvert.SerializeObject(obj, _settings), Encoding.UTF8, "application/json");
						var response = await client.PutAsync(apiRoute, stringContent);
						if (response != null)
						{

						}
					}

					break;
				}
				catch (Exception e)
				{
					continue;
				}
			}
		}

		public async void Delete(string apiRoute, object id, Dictionary<string, string> headers = null)
		{
			throw new NotImplementedException();
		}
	}
}
