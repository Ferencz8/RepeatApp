using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.GenericLibs.PCL.APICallers
{
	public class HttpClientExtensions
	{

		public static HttpClient GetAPIClient(string apiURL)
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri(apiURL);

			return client;
		}
	}
}
