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

namespace Repeat.Activities.Authentication
{
	public class ProgressDialogFragment : DialogFragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
			Cancelable = false;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			return base.OnCreateView(inflater, container, savedInstanceState);
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			//ProgressDialog dialog = new ProgressDialog(Activity, )
			return base.OnCreateDialog(savedInstanceState);
		}
	}
}