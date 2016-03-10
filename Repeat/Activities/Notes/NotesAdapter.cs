using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using Repeat.Entities;

namespace Repeat
{
	public class NotesAdapter : BaseAdapter
    {
        private List<Note> _notesList;
        Activity _activity;
        public NotesAdapter(Activity activity)
        {
            _notesList = Storage.GetItems();
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
    }
}