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

		public override List<Notebook> Get()
		{
			return _db.Table<Notebook>().Where(n => n.Deleted == false).ToList();
		}

		public Notebook GetByName(string notebookName)
		{
			return _db.Table<Notebook>().SingleOrDefault(n => n.Name.Equals(notebookName));
		}

		public List<Notebook> GetNotebooksWithNotesByLastModifiedDateOfNotes(DateTime lastModifiedDate)
		{
			var notebooks = _db.GetAllWithChildren<Notebook>();

			foreach(var notebook in notebooks)
			{
				if(notebook.Notes == null || notebook.Notes.Count == 0)
				{
					continue;
				}

				var requestedNotes = notebook.Notes.Where(n => n.ModifiedDate > lastModifiedDate).ToList();

				notebook.Notes.Clear();

				notebook.Notes.AddRange(requestedNotes);
			}

			return notebooks;
		}

		/// <summary>
		/// Perform logical Delete.
		/// </summary>
		/// <param name="objPrimaryKey"></param>
		/// <returns></returns>
		public override int Delete(object objPrimaryKey)
		{
			Notebook notebookDb = base.GetByID(objPrimaryKey);
			notebookDb.Deleted = true;
			notebookDb.DeletedDate = DateTime.UtcNow;
			notebookDb.ModifiedDate = DateTime.UtcNow;

			return base.Update(notebookDb);
		}

		public override int DeleteAll()
		{
			var notesDb = this.Get();
			foreach (var notebookDb in notesDb)
			{
				notebookDb.Deleted = true;
				notebookDb.DeletedDate = DateTime.UtcNow;
				notebookDb.ModifiedDate = DateTime.UtcNow;
				base.Update(notebookDb);
			}

			return 1;
		}

		//TODO::temp
		public int EraseAll()
		{
			return base.DeleteAll();
		}
	}
}
