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

		protected override void OnResume()
		{
			base.OnResume();

			//TODO:: here I could add Loading screen
			var app = App.Current;//just for testing
		}
	}
}