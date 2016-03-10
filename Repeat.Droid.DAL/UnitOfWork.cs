using Repeat.DAL.Repositories;
using Repeat.DAL.Repositories.Interfaces;
using SQLite.Net.Interop;

namespace Repeat.DAL
{
	public class UnitOfWork : IUnitOfWork
	{

		INotesRepository _notesRepository;
		INotebooksRepository _notebookRepository;

		static Db _db;

		public UnitOfWork(INotesRepository notesRepository, INotebooksRepository notebooksRepository, ISQLitePlatform sqlLitePlatform, string databasePath)
		{
			_notebookRepository = notebooksRepository;
			_notesRepository = notesRepository;

			_db = new Db(sqlLitePlatform, databasePath);
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

		public INotebooksRepository NotebooksRepository
		{
			get
			{
				if(_notebookRepository == null)
				{
					_notebookRepository = new NotebooksRepository(_db);
				}

				return _notebookRepository;
			}
		}
	}
}
