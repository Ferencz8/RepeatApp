using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.Logging;
using SQLite.Net;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repeat.Mobile.PCL
{

	public static class Util
	{

		public static ILog Log { get; set; }


		public static ISQLitePlatform SQLitePlatform { get; set; }

		public static string DatabasePath { get; set; }


		private static SQLiteConnection _connection;
		public static SQLiteConnection CreateDbConnection()
		{

			if (SQLitePlatform == null || string.IsNullOrWhiteSpace(DatabasePath))
				throw new Exception("Parameters SQLitePlatform and DatabasePath must be set before using this method.");

			_connection = new SQLiteConnection(SQLitePlatform, DatabasePath);

			_connection.CreateTable<Notebook>();
			_connection.CreateTable<Note>();
			if (_connection.Table<Notebook>().ToList().Count == 0)//check if there are 0 notebooks -> add a default one
			{
				_connection.Insert(new Notebook()
				{
					Id = Guid.NewGuid().ToString(),
					Name = "First Notebook",
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
				});
			}
			return _connection;
		}

		public static SQLiteConnection GetDbConnection()
		{
			if(_connection == null)
			{
				CreateDbConnection();
			}
			return _connection;
		}
	}
}
