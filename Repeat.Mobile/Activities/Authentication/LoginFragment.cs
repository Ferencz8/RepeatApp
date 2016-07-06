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
using System.Threading.Tasks;

namespace Repeat.Activities.Authentication
{
	public class LoginFragment : BaseFragment
	{
		private int _layoutToInflate;
		private EditText _username;
		private EditText _password;
		private ProgressBar _loginProgressBar;

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


			LinearLayout loginLayout = (LinearLayout)inflater.Inflate(_layoutToInflate, container, false);
			Button loginButton = loginLayout.FindViewById<Button>(Resource.Id.login_button);
			Button show_signupButotn = loginLayout.FindViewById<Button>(Resource.Id.show_signup);
			_username = loginLayout.FindViewById<EditText>(Resource.Id.login_username);
			_password = loginLayout.FindViewById<EditText>(Resource.Id.login_password);
			_loginProgressBar = loginLayout.FindViewById<ProgressBar>(Resource.Id.loginProgressBar);

			loginButton.Click += LoginButton_Click;
			show_signupButotn.Click += Show_signupButotn_Click;

			return loginLayout;
		}

		private void Show_signupButotn_Click(object sender, EventArgs e)
		{
			if (CheckIfFragmentExists(typeof(SignUpFragment).Name))
			{
				ShowExistingFragment(typeof(SignUpFragment).Name, false);
			}
			else
			{
				CreateNewFragment(new SignUpFragment(Resource.Layout.SignUp));
			}
		}

		private void LoginButton_Click(object sender, EventArgs e)
		{
			
			if (string.IsNullOrEmpty(_username.Text) || string.IsNullOrEmpty(_password.Text))
			{
				Toast.MakeText(Activity, "Username or password is not filled in", ToastLength.Short).Show();
			}
			else if(_password.Text.Trim().Length < 5)
			{
				Toast.MakeText(Activity, "Password must be at least 5 characters", ToastLength.Short).Show();
			}
			else {

				_loginProgressBar.Visibility = ViewStates.Visible;

				var mOverlayDialog = new Dialog(Activity, Android.Resource.Style.ThemePanel); //display an invisible overlay dialog to prevent user interaction and pressing back
				mOverlayDialog.SetCancelable(false);
				mOverlayDialog.Show();


				Task.Factory.StartNew(() =>
				{
					if (Login(_username.Text, _password.Text))
					{
						RemoveThisFragment();

						//StartNotesActivity
						Intent intent = new Intent(Activity, typeof(NotesActivity));
						StartActivity(intent);

						Activity.RunOnUiThread(() =>
						{
							_loginProgressBar.Visibility = ViewStates.Gone;
							mOverlayDialog.Dismiss();
						});
					}
					else
					{
						Activity.RunOnUiThread(() =>
						{
							_loginProgressBar.Visibility = ViewStates.Gone;
							mOverlayDialog.Dismiss();
							Toast.MakeText(Activity, "Failed to authenticate", ToastLength.Short).Show();
						});
					}
				});
			}
		}

		private bool Login(string username, string password)
		{
			return new UserAuthenticator().LogIn(username, password);
		}
	}
}