using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces;
using Repeat.NotebooksAPI.Infrastructure.Repositories;
using System.Data.Entity.Validation;

namespace Repeat.NotebooksAPI.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{

		Db _db = new Db();
		INotebooksRepository _notebooksRepository;
		INotesRepository _notesRepository;


		public INotebooksRepository NotebooksRepository
		{
			get
			{
				if(_notebooksRepository == null)
				{
					_notebooksRepository = new NotebooksRepository(_db);
				}

				return _notebooksRepository;
			}
		}

		public INotesRepository NotesRepository
		{
			get
			{
				if(_notesRepository == null)
				{
					_notesRepository = new NotesRepository(_db);
				}
				return _notesRepository;
			}
		}

		public void Save()
		{
			try
			{
				_db.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				foreach (var eve in e.EntityValidationErrors)
				{
					Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
				throw;
			}
		}
	}
}
