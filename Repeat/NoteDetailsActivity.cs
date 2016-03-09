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
using Repeat.Entities;

namespace Repeat
{
	[Activity (Label = "NoteDetailsActivity")]
	public class NoteDetailsActivity : Activity
	{
        protected override void OnCreate (Bundle bundle)
		{
            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetHomeButtonEnabled(true);
            base.OnCreate (bundle);

            // Create your application here
            SetContentView(Resource.Layout.NoteDetails);
            EditText txtNote = FindViewById<EditText>(Resource.Id.txtNote);
            EditText txtContent = FindViewById<EditText>(Resource.Id.txtContent);
            CheckBox checkBox = FindViewById<CheckBox>(Resource.Id.chkDone);

            Button addButton = FindViewById<Button>(Resource.Id.addButton);

            addButton.Click += delegate {
                Storage.AddItem(new Note() { Name = txtNote.Text, Content = txtContent.Text });
                Finish();
            };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }


        protected override void OnPause()
        {
            base.OnPause();
        }
    }
}