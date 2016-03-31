using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.DAL;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using System;

namespace Repeat.Mobile.Activities.Notes
{
	public class NotesAdapter : BaseAdapter
	{
		List<Note> _notesList;
		Activity _activity;
		INotesRepository notesRepisotory;
		public NotesAdapter(Activity activity, int notebookId = 0)
		{
			notesRepisotory = Kernel.Get<INotesRepository>();
			var notebooksRepository = Kernel.Get<INotebooksRepository>();
			if (notebookId == 0)
			{
				notebookId = notebooksRepository.Get().First().Id;
			}
			_notesList = notesRepisotory.GetNotesByNotebookId(notebookId);

			_activity = activity;
		}

		public override int Count
		{
			get
			{
				return _notesList.Count;
			}
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;// _notes.ElementAt(position);
		}

		public Note GetItemAtPosition(int position)
		{
			return _notesList.ElementAt(position);
		}

		public override long GetItemId(int position)
		{
			return _notesList[position].Id;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.NoteListItem, parent, false);

			var noteName = view.FindViewById<TextView>(Resource.Id.NoteName);
			var noteContent = view.FindViewById<TextView>(Resource.Id.NoteContent);

			noteName.Text = _notesList[position].Name;
			noteContent.Text = _notesList[position].Content;
			return view;
		}

		public void RefreshContent(int chosenNotebookId)
		{
			_notesList.Clear();
			_notesList = notesRepisotory.GetNotesByNotebookId(chosenNotebookId);
		}
	}
}