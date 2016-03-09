//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using System.Data;
//using Mono.Data.Sqlite;
//using System.IO;
//using Repeat.Entities;
//using Repeat.DAL;

//namespace Repeat
//{
//	public class Db
//	{
//		private string dbPath;
//		public Db(LocalPathAndroid path)
//		{
//			dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "sqlLiteDb.db3");

//			bool exists = File.Exists(dbPath);

//			if (!exists)
//			{
//				using (SqliteConnection conn = new SqliteConnection("Data Source=" + dbPath))
//				{

//					var commands = new[] {
//											@"CREATE TABLE Notes(
//											  Id INTEGER PRIMARY KEY AUTOINCREMENT,
//											  NotebookId INTEGER,
//											  Name varchar,
//											  Content varcahr,
//											  FOREIGN KEY (NotebookId) REFERENCES Notebooks(Id)
//											  ); ",
//											@"  
//												CREATE TABLE Notebooks(
//												Id INTEGER PRIMARY KEY AUTOINCREMENT,
//												Name varchar
//												)",
//											"INSERT INTO Notebooks values (1, 'First')",

//											"INSERT INTO NOTES values(1,1,'TEST', 'Content')",
//										};
//					// Open the database connection and create table with data
//					conn.Open();
//					foreach (var command in commands)
//					{
//						using (SqliteCommand cmd = conn.CreateCommand())
//						{
//							cmd.CommandText = command;
//							var rowcount = cmd.ExecuteNonQuery();
//							Console.WriteLine("\tExecuted " + command);
//						}
//					}
//				}
//			}
//		}


//		public List<Note> SelectQuery(string query)
//		{
//			List<Note> notes = new List<Note>();
//			try
//			{

//				using (SqliteConnection conn = new SqliteConnection("Data Source=" + dbPath))
//				using (SqliteCommand cmd = conn.CreateCommand())
//				{
//					conn.Open();  //Initiate connection to the db
//					cmd.CommandText = query;  //set the passed query


//					var r = cmd.ExecuteReader();

//					while (r.Read())
//					{
//						notes.Add(new Note() { Id = r.GetInt32(0), NotebookId = r.GetInt32(1), Name = r.GetString(2), Content = r.GetString(3) });
//					}
//				}
//			}
//			catch (SqliteException ex)
//			{
//				//Add your exception code here.

//			}
//			return notes;
//		}


//		public List<Notebook> SelectNotebooks(string query)
//		{
//			List<Notebook> notebooks = new List<Notebook>();
//			try
//			{

//				using (SqliteConnection conn = new SqliteConnection("Data Source=" + dbPath))
//				using (SqliteCommand cmd = conn.CreateCommand())
//				{
//					conn.Open();  //Initiate connection to the db
//					cmd.CommandText = query;  //set the passed query


//					var r = cmd.ExecuteReader();

//					while (r.Read())
//					{
//						notebooks.Add(new Notebook() { Id = r.GetInt32(0), Name = r.GetString(1) });
//					}
//				}
//			}
//			catch (SqliteException ex)
//			{
//				//Add your exception code here.

//			}
//			return notebooks;
//		}


//		public int InsertQuery(string query, List<SqliteParameter> parameters)
//		{
//			try
//			{
//				using (SqliteConnection conn = new SqliteConnection("Data Source=" + dbPath))
//				using (SqliteCommand cmd = conn.CreateCommand())
//				{
//					conn.Open();  //Initiate connection to the db
//					cmd.CommandText = query;  //set the passed query

//					cmd.Parameters.AddRange(parameters.ToArray());
//					int result = cmd.ExecuteNonQuery();
//					return result;
//				}
//			}
//			catch (SqliteException ex)
//			{
//				//Add your exception code here.
//				return 0;
//			}
//		}
//	}
//}