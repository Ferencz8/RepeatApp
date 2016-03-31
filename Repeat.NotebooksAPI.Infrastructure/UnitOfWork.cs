using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces;
using Repeat.NotebooksAPI.Infrastructure.Repositories;

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
			_db.SaveChanges();
		}
	}
}
