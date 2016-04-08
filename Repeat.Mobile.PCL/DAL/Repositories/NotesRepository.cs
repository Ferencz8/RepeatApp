using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.DAL.Repositories
{
	public class NotesRepository : GenericRepository<Note>, INotesRepository
	{
		SQLiteConnection _db;

		public NotesRepository(SQLiteConnection db)
			: base(db)
		{
			_db = db;
		}

		public List<Note> GetNotesByNotebookId(Guid notebookId)
		{
			string notebookIdAsString = notebookId.ToString();
			return _db.Table<Note>().Where(n => n.NotebookId.Equals(notebookIdAsString)).ToList();
		}
	}
}
