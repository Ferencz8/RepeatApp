using Newtonsoft.Json;
using Repeat.GenericLibs.PCL.APICallers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.GenericLibs.PCL.APICallers
{
	public class GenericAPICaller<T> : IGenericAPICaller<T>
	{
		private string _apiURL;

		public GenericAPICaller(string apiURL)
		{
			_apiURL = apiURL;
		}

		public async void Add(string apiRoute, T obj)
		{
			throw new NotImplementedException();
		}

		public async void Delete(string apiRoute, object id)
		{
			throw new NotImplementedException();
		}

		public async Task<List<T>> Get(string apiRoute)
		{
			List<T> elements = new List<T>();
			try
			{
				var client = HttpClientExtensions.GetAPIClient(_apiURL);
				var response = await client.GetAsync(apiRoute);
				if (response != null)
				{
					string str = await response.Content.ReadAsStringAsync();
					elements = JsonConvert.DeserializeObject<List<T>>(str);
				}
			}
			catch (Exception e)
			{
				//TODO LOG
			}
			return elements;
		}

		public async Task<T> GetById(string apiRoute, object id)
		{
			throw new NotImplementedException();
		}

		public async void Update(string apiRoute, T obj)
		{
			throw new NotImplementedException();
		}
	}
}
