using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Repeat.Mobile;

namespace Repeat.AppLayer
{
	public abstract class ActivityBase : Activity
	{
		ProgressDialog progress;
		protected EventHandler<EventArgs> initializedEventHandler;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			RunAppInitializedEventHandler();
		}

		protected override void OnResume()
		{
			base.OnResume();

			RunAppInitializedEventHandler();
		}

		private void RunAppInitializedEventHandler()
		{
			if (!App.Current.IsInitialized)
			{
				// show the loading overlay on the UI thread
				progress = ProgressDialog.Show(this, "Loading", "Please Wait...", true);

				// when the app has initialized, hide the progress bar and call Finished Initialzing
				initializedEventHandler = (s, e) =>
				{
					// call finished initializing so that any derived activities have a chance to do work
					RunOnUiThread(() =>
					{
						this.FinishedInitializing();
						// hide the progress bar
						if (progress != null)
							progress.Dismiss();
					});
				};
				App.Current.Initialized += initializedEventHandler;

			}
		}

		protected override void OnPause()
		{
			base.OnPause();

			// in the case of rotation before the app is fully intialized, we have
			// to remove our intialized event handler, and dismiss the progres. otherwise
			// we'll get multiple Initialized handler calls and a window leak.
			if (this.initializedEventHandler != null)
				App.Current.Initialized -= initializedEventHandler;
			if (progress != null)
				progress.Dismiss();
		}

		/// <summary>
		/// Override this method to perform tasks after the app class has fully initialized
		/// </summary>
		protected abstract void FinishedInitializing();
	}
}