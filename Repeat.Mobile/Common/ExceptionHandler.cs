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

namespace Repeat.Common
{
	public static class ExceptionHandler
	{

		public static Activity Activity { get; set; }

		public static void DisplayFatalErrorMessage()
		{
			Toast.MakeText(Activity, "Application will unfortunately crash. Smile and have a good day!", ToastLength.Short);
		}
	}
}