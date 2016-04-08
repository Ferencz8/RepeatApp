using Repeat.Mobile.PCL.DAL.Entities;
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

		private static SQLiteConnection _connection;

		public static void CreateConnection(ISQLitePlatform sqlitePlatform, string databasePath)
		{
			if (_connection == null)
			{
				_connection = new SQLiteConnection(sqlitePlatform, databasePath);

				_connection.CreateTable<Notebook>();
				_connection.CreateTable<Note>();
				if (_connection.Table<Notebook>().ToList().Count == 0)//check if there are 0 notebooks add a default one
				{
					_connection.Insert(new Notebook()
					{
						Id = Guid.NewGuid().ToString(),
						Name = "First Notebook",
						CreatedDate = DateTime.Now,
						ModifiedDate = DateTime.Now,
					});
				}

			}
		}

		public static SQLiteConnection GetDbConnection()
		{
			return _connection;
		}
	}
}
