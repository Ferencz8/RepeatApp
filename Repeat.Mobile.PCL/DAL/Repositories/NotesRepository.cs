﻿using Repeat.Mobile.PCL.DAL.Entities;
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

		public override List<Note> Get()
		{
			return _db.Table<Note>().Where(n => !n.Deleted).ToList();
		}

		public List<Note> GetNotesByNotebookId(Guid notebookId)
		{
			string notebookIdAsString = notebookId.ToString();
			return _db.Table<Note>().Where(n => n.NotebookId.Equals(notebookIdAsString)).ToList();
		}

		/// <summary>
		/// Perform logical Delete.
		/// </summary>
		/// <param name="objPrimaryKey"></param>
		/// <returns></returns>
		public override int Delete(object objPrimaryKey)
		{
			Note noteDb = base.GetByID(objPrimaryKey);
			noteDb.Deleted = true;
			noteDb.DeletedDate = DateTime.UtcNow;
			noteDb.ModifiedDate = DateTime.UtcNow;

			return base.Update(noteDb);
		}

		public override int DeleteAll()
		{
			var notesDb = this.Get();
			foreach(var noteDb in notesDb)
			{
				noteDb.Deleted = true;
				noteDb.DeletedDate = DateTime.UtcNow;
				noteDb.ModifiedDate = DateTime.UtcNow;
				base.Update(noteDb);
			}

			return 1;
		}
	}
}
