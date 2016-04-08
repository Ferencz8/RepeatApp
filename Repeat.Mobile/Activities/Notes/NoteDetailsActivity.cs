using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.PCL.DependencyManagement;
using System;

namespace Repeat.Mobile.Activities.Notes
{
	[Activity (Label = "NoteDetailsActivity")]
	public class NoteDetailsActivity : Activity
	{

		INotesRepository _notesRepository;
		Guid chosenNotebookId;

        protected override void OnCreate (Bundle bundle)
		{
			chosenNotebookId = Guid.Parse(Intent.GetStringExtra("notebookId"));

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetHomeButtonEnabled(true);
            base.OnCreate (bundle);

            // Create your application here
            SetContentView(Resource.Layout.NoteDetails);
            EditText txtNote = FindViewById<EditText>(Resource.Id.txtNote);
            EditText txtContent = FindViewById<EditText>(Resource.Id.txtContent);
            CheckBox checkBox = FindViewById<CheckBox>(Resource.Id.chkDone);

            Button addButton = FindViewById<Button>(Resource.Id.addButton);


			_notesRepository = Kernel.Get<INotesRepository>();
            addButton.Click += delegate {
				_notesRepository.Add(new Note()
				{
					Id = Guid.NewGuid().ToString(),
					Name = txtNote.Text,
					Content = txtContent.Text,
					NotebookId = chosenNotebookId.ToString(),
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
				});
				Finish();
            };
        }

        //protected override void OnDestroy()
        //{
        //    base.OnDestroy();
        //}

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //}


        //protected override void OnPause()
        //{
        //    base.OnPause();
        //}
    }
}