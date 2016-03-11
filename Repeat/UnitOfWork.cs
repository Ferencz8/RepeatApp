using Ninject.Parameters;
using Repeat.DAL.Repositories;
using Repeat.DAL.Repositories.Interfaces;
using Repeat.Droid.DAL.DependencyManagement;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using System.IO;

namespace Repeat.DAL
{
	public class UnitOfWork : IUnitOfWork
	{

		INotesRepository _notesRepository;
		INotebooksRepository _notebookRepository;

		Db _db;

		public UnitOfWork()
		{

			string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "repeatDb.db3");
			_db = new Db(new SQLitePlatformAndroid(), path);
		}

		public INotesRepository NotesRepository
		{
			get
			{
				if(_notesRepository == null)
				{
					_notesRepository = Kernel.Get<INotesRepository>(new ConstructorArgument("db", _db));//new NotesRepository(_db);
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
					_notebookRepository = Kernel.Get<INotebooksRepository>(new ConstructorArgument("db", _db)); ;//new NotebooksRepository(_db);
				}

				return _notebookRepository;
			}
		}
	}
}
