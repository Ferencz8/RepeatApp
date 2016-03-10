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
using Mono.Data.Sqlite;
using Repeat.Entities;
using Repeat.Droid.DAL;
using System.IO;
using Repeat.DAL;
using SQLite.Net.Platform.XamarinAndroid;

namespace Repeat
{
	public static class Storage
	{
		private static Db db;


		static Storage()
		{

			Util.File = new AndroidFile();
			string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "repeatDb.db3");
			db = new Db(new SQLitePlatformAndroid(), path);
		}

		internal static List<Notebook> GetNotebooks()
		{
			return db.Get<Notebook>();
		}

		public static void AddItem(Note item)
		{
			db.Add(item);
		}

		public static List<Note> GetItems()
		{
			return db.Get<Note>();
		}
	}
}