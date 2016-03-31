using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.DAL.Repositories
{
	public class NotesRepository : GenericRepository<Note>, INotesRepository
	{
		Db _db;

		public NotesRepository(Db db)
			: base(db)
		{
			_db = db;
		}

		public List<Note> GetNotesByNotebookId(int notebookId)
		{
			return _db.Table<Note>().Where(n => n.NotebookId.Equals(notebookId)).ToList();
		}
	}
}
