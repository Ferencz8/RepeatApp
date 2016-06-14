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

		public List<Notebook> GetForUser(string userId)
		{
			return _db.Table<Notebook>().Where(n => n.Deleted == false && n.UserId.Equals(userId)).ToList();
		}

		public Notebook GetByName(string notebookName)
		{
			return _db.Table<Notebook>().SingleOrDefault(n => n.Name.Equals(notebookName));
		}

		public List<Notebook> GetNotebooksWithNotesByLastModifiedDateOfNotes(DateTime lastModifiedDate, string userId)
		{
			var notebooks = _db.GetAllWithChildren<Notebook>(n => n.UserId.Equals(userId));

			foreach(var notebook in notebooks)
			{
				if(notebook.Notes == null || notebook.Notes.Count == 0)
				{
					continue;
				}

				var requestedNotes = notebook.Notes.Where(n => DateTime.Compare(n.ModifiedDate, lastModifiedDate) > 0 ).ToList();
				PCL.DependencyManagement.Kernel.Get<Logging.ILog>().Info(Guid.Empty, notebook.Notes[0].ModifiedDate.ToString() + " " + lastModifiedDate.ToString());
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

		public int EraseAll(string userId)
		{
			var notebooks = this.GetForUser(userId);
			foreach(var notebookId in notebooks.Select(n => n.Id))
			{
				base.Delete(notebookId);
			}
			return notebooks.Count;
		}
	}
}
