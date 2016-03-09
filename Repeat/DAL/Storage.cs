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
using Repeat.DAL;
using System.IO;

namespace Repeat
{
	public static class Storage
	{
		//static List<string> items = new List<string> { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
		private static Db db;


		static Storage()
		{

			var localPathAndroidObj = new LocalPathAndroid();
			localPathAndroidObj.OSPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sqlLiteDb.db3");
			db = new Db(localPathAndroidObj);
		}

	

		internal static List<Notebook> GetNotebooks()
		{
			return db.SelectNotebooks("select * from notebooks;");
		}

		public static void AddItem(Note item)
		{
			if (item != null)
			{

				List<SqliteParameter> parameters = new List<SqliteParameter>() {
					new SqliteParameter("@name", item.Name),
					new SqliteParameter("@content", item.Content)
				};
				db.InsertQuery("insert into notes(Name, Content) values(@name, @content)", parameters);
				//items.Add(item);
			}
		}

		public static List<Note> GetItems()
		{
			return db.SelectQuery("select * from notes;");
		}
	}
}