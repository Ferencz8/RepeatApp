using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Views;
using Android.Widget;
using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.DAL;
using Repeat.Mobile.PCL;

namespace Repeat.Mobile.Activities.Notebooks
{
	public class NotebooksAdapter : BaseAdapter
	{
		List<Notebook> _notebooksList;
		Activity _activity;
		IUnitOfWork _unitOfWork;

		public NotebooksAdapter(Activity activity)
		{
			_unitOfWork = Kernel.Get<IUnitOfWork>();
			_notebooksList = _unitOfWork.NotebooksRepository.GetForUser(Session.LoggedInUser.Id);
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
			return 0;// _notebooksList[position].Id;
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
			_notebooksList = _unitOfWork.NotebooksRepository.GetForUser(Session.LoggedInUser.Id);
		}
	}
}