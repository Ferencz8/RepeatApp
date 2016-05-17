using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Repeat.Mobile.PCL.Common;
using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.PCL.DependencyManagement;
using System;

namespace Repeat.Mobile.Activities.Notes
{
	[Activity(Label = "NoteDetailsActivity")]
	public class NoteDetailsActivity : Activity
	{
		EditText _txtNote;
		EditText _txtContent;

		INotesRepository _notesRepository;
		Guid chosenNotebookId;
		Note _noteToBeEdited;

		//TODO:: a possible resason for not synching is that here a date is saved in utc and in web client in gmt + 3 format
		protected override void OnCreate(Bundle bundle)
		{
			chosenNotebookId = Guid.Parse(Intent.GetStringExtra("notebookId"));
			RequestWindowFeature(WindowFeatures.ActionBar);

			ActionBar.SetHomeButtonEnabled(true);
			base.OnCreate(bundle);


			// Create your application here
			SetContentView(Resource.Layout.NoteDetails);
			_txtNote = FindViewById<EditText>(Resource.Id.txtNote);
			_txtContent = FindViewById<EditText>(Resource.Id.txtContent);
			Button deleteButton = FindViewById<Button>(Resource.Id.deleteNoteButton);

			Button addEditButton = FindViewById<Button>(Resource.Id.addEditButton);


			_notesRepository = Kernel.Get<INotesRepository>();


			if (Intent.GetStringExtra("action").Equals("ADD"))
			{
				addEditButton.Click += AddButton_Click;
				
				deleteButton.Visibility = ViewStates.Invisible;
			}
			else//EDIT
			{
				string jsonNote = Intent.GetStringExtra("note");
				if (jsonNote == null)
				{ throw new Exception("Note to be edited was not passed"); }

				_noteToBeEdited = ObjectConverter.ToObject<Note>(jsonNote);

				_txtNote.Text = _noteToBeEdited.Name;
				_txtContent.Text = _noteToBeEdited.Content;

				addEditButton.Text = "Save Changes";
				addEditButton.Click += EditButton_Click;
			}

			deleteButton.Click += DeleteButton_Click;
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var builder = new AlertDialog.Builder(this);
			builder.SetTitle("Delete note");
			builder.SetMessage("Are you sure you want to delete this note ?");
			builder.SetPositiveButton("Yeah", (alertDialogSender, args) =>
			{
				_notesRepository.Delete(_noteToBeEdited.Id);
				Finish();
			});
			builder.SetNegativeButton("Nope", (alertDialogSender, args) =>
			{
				Finish();
			});
			builder.Show();
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			_noteToBeEdited.Name = _txtNote.Text;
			_noteToBeEdited.Content = _txtContent.Text;
			_noteToBeEdited.ModifiedDate = DateTime.UtcNow;

			_notesRepository.Update(_noteToBeEdited);
			Finish();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			_notesRepository.Add(new Note()
			{
				Id = Guid.NewGuid().ToString(),
				Name = _txtNote.Text,
				Content = _txtContent.Text,
				NotebookId = chosenNotebookId.ToString(),
				CreatedDate = DateTime.UtcNow,
				ModifiedDate = DateTime.UtcNow,
			});
			Finish();
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