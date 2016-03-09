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
using Java.Lang;
using Repeat.Entities;

namespace Repeat
{
	public class SideMenuAdapter : BaseAdapter
	{
		private List<Notebook> _notebooksList;
		Activity _activity;
		public SideMenuAdapter(Activity activity)
		{
			_notebooksList = Storage.GetNotebooks();
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
			var view = convertView ?? _activity.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, parent, false);
			return view;
		}
	}
}