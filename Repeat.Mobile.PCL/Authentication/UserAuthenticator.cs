using Repeat.Mobile.PCL.Common;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp;

namespace Repeat.Mobile.PCL.Authentication
{
	public class UserAuthenticator
	{
		dynamic _api;
		ILog _log;

		public UserAuthenticator()
		{
			_log = Kernel.Get<ILog>();
		}

		public bool LogIn(string username, string password)
		{
			for (int i = 0; i < 3; i++)
			{
				try
				{
					_api = new API(Configs.UserAPP_APIKey);
					var loginResult = _api.User.Login(login: username, password: password);

					Session.LoggedInUser = new User();


					var user = _api.User.Get()[0];

					AnalyzeDynamicUserAndPopulateStronglyTypedUser(loginResult, Session.LoggedInUser);

					AnalyzeDynamicUserAndPopulateStronglyTypedUser(user, Session.LoggedInUser);
				}
				catch (Exception e)
				{
					_log.Exception(Guid.Empty, e, string.Format("Login failed for username {0} - and pass {1}", username, password));
				}
			}

			return !string.IsNullOrEmpty(Session.LoggedInUser.Id);
		}

		public void LogOut()
		{
			try
			{
				_api.User.Logout();
			}
			catch (Exception e)
			{
				_log.Exception(Guid.Empty, e, "Log Out Failed");
			}
		}

		public bool SignUp(string username, string password)
		{
			try
			{

				var newUser = _api.User.Save(login: username, password: password);

				return newUser != null ? true : false;
			}
			catch (Exception e)
			{
				_log.Exception(Guid.Empty, e, string.Format("SignUp failed for username {0} - and pass {1}", username, password));
				return false;
			}
		}

		private void AnalyzeDynamicUserAndPopulateStronglyTypedUser(dynamic obj, User user)
		{
			if (obj is UserApp.CodeConventions.ObjectAccessDecorator)
			{
				foreach (var element in obj)
				{
					if (element.Key.Equals("token"))
					{
						user.Token = element.Value;
						return;
					}
					else if (element.Key.Equals("user_id"))
					{
						user.Id = element.Value;
					}
					else if (element.Key.Equals("first_name"))
					{
						user.FirstName = element.Value;
					}
					else if (element.Key.Equals("last_name"))
					{
						user.LastName = element.Value;
					}
					else if (element.Key.Equals("created_at"))
					{
						user.CreatedAt = new DateTime((long)element.Value);
					}
				}


				foreach (var permission in obj.Permissions)
				{
					if (permission.Value.value)
						user.Permissions.Add(permission.Key);
				}
			}
		}
	}
}
