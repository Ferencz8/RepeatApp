using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Repeat.NotebooksAPI
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "Complex",
				routeTemplate: "api/notebooks/{id}/notes/{date}",
				defaults: new {controller = "Notebooks", action = "Notes", id = @"([A-Za-z0-9-]){36,36}", date = @"([0-9-]){6,20}" }
				);
		}
	}
}
