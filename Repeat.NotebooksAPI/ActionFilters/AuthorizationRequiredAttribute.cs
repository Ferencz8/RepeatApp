using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UserApp;

namespace Repeat.NotebooksAPI.ActionFilters
{
	public class AuthorizationRequiredAttribute : ActionFilterAttribute
	{

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var headers = actionContext.Request.Headers;
			bool unauthorized = false;

			for (int i = 0; i < 3; i++)
			{
				try
				{
					dynamic _api = new API("570d145f4ff98", headers.Authorization.Scheme);
					var check = _api.User.Get();

					if (check == null)
					{
						unauthorized = true;
					}
					else {
						unauthorized = false;
						break;
					}
				}
				catch (Exception e)
				{
					unauthorized = true;
				}
			}

			if (unauthorized)
			{
				actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			}
		}
	}
}