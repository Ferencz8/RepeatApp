using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Extensions;

namespace Repeat.Mobile.PCL.DAL.Repositories
{
	public class NotebooksRepository : GenericRepository<Notebook>, INotebooksRepository
	{
		SQLiteConnection _db;

		public NotebooksRepository(SQLiteConnection db)
			: base(db)
		{
			_db = db;
		}

		public Notebook GetByName(string notebookName)
		{
			return _db.Table<Notebook>().SingleOrDefault(n => n.Name.Equals(notebookName));
		}

		public List<Notebook> GetNotebooksWithNotesByLastModifiedDateForNotes(DateTime lastModifiedDate)
		{
			var notebooks = _db.GetAllWithChildren<Notebook>();

			foreach(var notebook in notebooks)
			{
				if(notebook.Notes == null)
				{
					continue;
				}

				var requestedNotes = notebook.Notes.Where(n => n.ModifiedDate > lastModifiedDate).ToList();

				notebook.Notes.Clear();

				notebook.Notes.AddRange(requestedNotes);
			}

			return notebooks;
		}
	}
}
