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
using Repeat.Mobile.PCL.DAL.Entities;

namespace Repeat.Mobile.Sync.DTOs
{
	public class DataToBeSynched : SyncRequest
	{

		public List<Notebook> Notebooks { get; set; }
	}
}