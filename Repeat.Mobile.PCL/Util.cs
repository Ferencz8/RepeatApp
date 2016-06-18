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
						
			return _connection;
		}

		public static void PrepareDatabaseForFirstTimeUse()
		{
			//used to create a first notebook if one does not exist
			if (Session.LoggedInUser == null || string.IsNullOrEmpty(Session.LoggedInUser.Id))
			{
				throw new Exception("Session.LoggedInUser is not set");
			}

			SQLiteConnection connection = GetDbConnection();

			if (!Is_DatabaseSchema_Created(connection))
			{
				CreateDatabaseSchema(connection);
			}

			if (connection.Table<Notebook>().Where(nb => nb.UserId.Equals(Session.LoggedInUser.Id)).ToList().Count == 0)//check if for current user there are 0 notebooks -> add a default one
			{
				connection.Insert(new Notebook()
				{
					Id = Guid.NewGuid().ToString(),
					Name = "First Notebook",
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
					UserId = Session.LoggedInUser.Id,
				});
			}
		}

		public static SQLiteConnection GetDbConnection()
		{
			if(_connection == null)
			{
				CreateDbConnection();
			}
			return _connection;
		}

		private static bool Is_DatabaseSchema_Created(SQLiteConnection conn)
		{
			var notebookTableInfo = conn.GetTableInfo("Notebook");
			var noteTableInfo = conn.GetTableInfo("Note");

			return notebookTableInfo.Count == 0 || noteTableInfo.Count == 0 ? false : true;
		}
		
		private static void CreateDatabaseSchema(SQLiteConnection connection)
		{
			connection.CreateTable<Notebook>();
			connection.CreateTable<Note>();
		}
	}
}
