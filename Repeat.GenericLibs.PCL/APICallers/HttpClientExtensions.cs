using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.GenericLibs.PCL.APICallers
{
	public static class HttpClientExtensions
	{

		public static HttpClient GetAPIClient(string apiURL)
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri(apiURL);

			return client;
		}

		public static void AddHeaders(this HttpClient client, Dictionary<string, string> headers)
		{
			if (headers != null)
			{
				foreach (var header in headers)
				{
					client.DefaultRequestHeaders.Add(header.Key, header.Value);
				}
			}
		}
	}
}
