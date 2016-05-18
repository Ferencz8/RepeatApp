using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json;
using Repeat.Activities.Notes;
using Repeat.Mobile.PCL.Authentication;

namespace Repeat.Activities.Authentication
{
	public class LoginFragment : BaseFragment
	{
		private int _layoutToInflate;
		private EditText _username;
		private EditText _password;

		public LoginFragment(int layoutId)
		{
			_layoutToInflate = layoutId;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);


			LinearLayout loginLayout = (LinearLayout)inflater.Inflate(_layoutToInflate, container);
			Button loginButton = loginLayout.FindViewById<Button>(Resource.Id.login_button);
			_username = loginLayout.FindViewById<EditText>(Resource.Id.login_username);
			_password = loginLayout.FindViewById<EditText>(Resource.Id.login_password);

			loginButton.Click += LoginButton_Click;

			return base.OnCreateView(inflater, container, savedInstanceState);
		}

		private void LoginButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(_username.Text) || string.IsNullOrEmpty(_password.Text))
			{
				Toast.MakeText(Activity, "Username or password is not filled in", ToastLength.Short);
			}

			if (Login(_username.Text, _password.Text))
			{
				RemoveThisFragment();

				//StartNotesActivity
				Intent intent = new Intent(Activity, typeof(NotesActivity));
				StartActivity(intent);
			}
			else
			{
				Toast.MakeText(Activity, "Failed to authenticate", ToastLength.Short);
			}
		}

		private bool Login(string username, string password)
		{
			return new UserAuthenticator().LogIn(username, password);
		}
	}
}