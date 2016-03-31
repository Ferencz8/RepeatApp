using Repeat.Mobile.PCL.DAL.Entities;
using SQLite.Net;
using SQLite.Net.Interop;
using System.Linq;

namespace Repeat.Mobile.PCL.DAL
{
	public class Db : SQLiteConnection
	{
		public Db(ISQLitePlatform sqlLitePlatform, string path) : base(sqlLitePlatform, path)
		{
			Initialize();
		}

		private void Initialize()
		{
			CreateTable<Notebook>();
			CreateTable<Note>();
			if (Table<Notebook>().ToList().Count == 0)//check if there are 0 notebooks add a default one
			{
				Insert(new Notebook()
				{
					Name = "First Notebook",
				});
			}
		}
	}
}
