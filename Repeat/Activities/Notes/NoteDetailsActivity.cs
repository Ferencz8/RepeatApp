using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Repeat.DAL.Entities;
using Repeat.DAL.Repositories.Interfaces;
using Repeat.Droid.DAL.DependencyManagement;

namespace Repeat
{
	[Activity (Label = "NoteDetailsActivity")]
	public class NoteDetailsActivity : Activity
	{

		INotesRepository _notesRepository;

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


			_notesRepository = Kernel.Get<INotesRepository>();
            addButton.Click += delegate {
				_notesRepository.Add(new Note() { Name = txtNote.Text, Content = txtContent.Text });
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