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
using System.Threading.Tasks;
using Repeat.Activities.Notes;
using Repeat.Mobile.PCL.Authentication;

namespace Repeat.Activities.Authentication
{
	public class SignUpFragment : BaseFragment
	{
		private int _layoutToInflate;
		private EditText _username;
		private EditText _password;
		private EditText _email;
		private ProgressBar _signupProgressBar;

		public SignUpFragment(int layoutId)
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


			LinearLayout signupLayout = (LinearLayout)inflater.Inflate(_layoutToInflate, container, false);
			Button signUpButton = signupLayout.FindViewById<Button>(Resource.Id.signup_buton);
			Button loginButton = signupLayout.FindViewById<Button>(Resource.Id.showlogin_button);
			_username = signupLayout.FindViewById<EditText>(Resource.Id.signup_username);
			_password = signupLayout.FindViewById<EditText>(Resource.Id.signup_password);
			_email = signupLayout.FindViewById<EditText>(Resource.Id.signup_email);

			_signupProgressBar = signupLayout.FindViewById<ProgressBar>(Resource.Id.signupProgressBar);

			signUpButton.Click += SignUpButton_Click;
			loginButton.Click += LoginButton_Click;

			return signupLayout;// base.OnCreateView(inflater, container, savedInstanceState);
		}

		private void LoginButton_Click(object sender, EventArgs e)
		{
			RemoveThisFragment();

			ShowExistingFragment(typeof(LoginFragment).Name, false);
		}

		private void SignUpButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(_username.Text) || string.IsNullOrEmpty(_password.Text) || string.IsNullOrEmpty(_email.Text))
			{
				Toast.MakeText(Activity, "All fields are mandatory", ToastLength.Short).Show();
			}
			else if (_password.Text.Trim().Length < 5)
			{
				Toast.MakeText(Activity, "Password must be at least 5 characters", ToastLength.Short);
			}
			else {


				_signupProgressBar.Visibility = ViewStates.Visible;

				var mOverlayDialog = new Dialog(Activity, Android.Resource.Style.ThemePanel); //display an invisible overlay dialog to prevent user interaction and pressing back
				mOverlayDialog.SetCancelable(false);
				mOverlayDialog.Show();


				Task.Factory.StartNew(() =>
				{
					if (SignUp(_username.Text, _password.Text, _email.Text))
					{
						Activity.RunOnUiThread(() =>
						{
							_signupProgressBar.Visibility = ViewStates.Gone;

							mOverlayDialog.Cancel();

							ShowExistingFragment(typeof(LoginFragment).Name, false);
						});
					}
					else
					{
						Activity.RunOnUiThread(() =>
						{
							_signupProgressBar.Visibility = ViewStates.Gone;

							mOverlayDialog.Cancel();

							Toast.MakeText(Activity, "Failed to signup", ToastLength.Short);
						});
					}
				});
			}
		}

		private bool SignUp(string username, string password, string email)
		{
			return new UserAuthenticator().SignUp(username, password, email);
		}
	}
}