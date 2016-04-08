using System.IO;
using Ninject.Parameters;
using Repeat.Mobile.PCL.DAL;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.PCL.DependencyManagement;
using SQLite.Net;

namespace Repeat.Mobile.PCL.DAL
{
	public class UnitOfWork : IUnitOfWork
	{

		INotesRepository _notesRepository;
		INotebooksRepository _notebookRepository;
		SQLiteConnection _db;

		public UnitOfWork()
		{
			_db = Util.GetDbConnection();
		}

		public INotesRepository NotesRepository
		{
			get
			{
				if(_notesRepository == null)
				{
					_notesRepository = Kernel.Get<INotesRepository>(new ConstructorArgument("db", _db));
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
					_notebookRepository = Kernel.Get<INotebooksRepository>(new ConstructorArgument("db", _db)); ;
				}

				return _notebookRepository;
			}
		}
	}
}
