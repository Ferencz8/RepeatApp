using Repeat.DAL.Entities;
using Repeat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.DAL.Repositories
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
