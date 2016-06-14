using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Repeat.Activities.Authentication;
using Repeat.Activities.Notes;
using Repeat.AppLayer;
using Repeat.Mobile.PCL;

namespace Repeat.Mobile
{
	[Activity(Label = "Repeat", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ActivityBase
	{
		

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			if (Session.LoggedInUser != null)
			{
				Intent intent = new Intent(this, typeof(NotesActivity));
				StartActivity(intent);
				Finish();
			}
			else {

				LoginFragment loginFragment = new LoginFragment(Resource.Layout.LogIn);
				Bundle bundle2 = new Bundle();
				bundle2.PutString("hi", "acsacas");

				loginFragment.Arguments = bundle2;

				LinearLayout view = new LinearLayout(this);
				int id = Resource.Id.mainActivity;
				int id2 = view.RootView.Id;
				NavigateTo(id, loginFragment, loginFragment.GetType().Name);
			}
		}

		public void NavigateTo(int currentViewToBeReplacedId, Fragment newFragment, string tag)
		{			
			FragmentTransaction ft = FragmentManager.BeginTransaction();

			ft.Replace(currentViewToBeReplacedId, newFragment, tag);
			ft.AddToBackStack(null);

			ft.Commit();
		}


		protected override void FinishedInitializing()
		{

		}
	}
}

