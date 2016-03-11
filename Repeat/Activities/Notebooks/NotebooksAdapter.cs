using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using Repeat.DAL.Entities;
using Repeat.Droid.DAL.DependencyManagement;
using Repeat.DAL.Repositories.Interfaces;
using Repeat.DAL;

namespace Repeat
{
	public class NotebooksAdapter : BaseAdapter
	{
		List<Notebook> _notebooksList;
		Activity _activity;
		INotebooksRepository notebooksRepository;

		public NotebooksAdapter(Activity activity)
		{
			notebooksRepository = Kernel.Get<INotebooksRepository>();
			_notebooksList = notebooksRepository.Get();
			_activity = activity;
		}

		public override int Count
		{
			get
			{
				return _notebooksList.Count;
			}
		}

		public override Java.Lang.Object GetItem(int position)
		{
			throw new NotImplementedException();
		}

		public Notebook GetItemAtPosition(int position)
		{
			return _notebooksList.ElementAt(position);
		}

		public override long GetItemId(int position)
		{
			return _notebooksList[position].Id;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.NotebookListItem, parent, false);

			var notebookName = view.FindViewById<TextView>(Resource.Id.NotebookName);
			notebookName.Text = _notebooksList[position].Name;

			return view;
		}

		public void RefreshContent()
		{
			_notebooksList = notebooksRepository.Get();
		}
	}
}