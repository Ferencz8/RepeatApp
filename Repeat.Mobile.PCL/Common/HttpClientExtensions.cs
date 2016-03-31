using System;
using System.Net.Http;

namespace Repeat.Common
{
	public class HttpClientExtensions
	{
		private static string APIUrl = "http://www.repeat.somee.com/";

		public static HttpClient GetAPIClient()
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri(APIUrl);

			return client;
		}
	}
}