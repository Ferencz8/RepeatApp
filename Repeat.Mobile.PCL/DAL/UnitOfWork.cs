using System.IO;
using Ninject.Parameters;
using Repeat.Mobile.PCL.DAL;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.PCL.DependencyManagement;
using SQLite.Net;
using System;
using Repeat.Mobile.PCL.DAL.Repositories;

namespace Repeat.Mobile.PCL.DAL
{
	public class UnitOfWork : IUnitOfWork
	{

		INotesRepository _notesRepository;
		INotebooksRepository _notebookRepository;
		SQLiteConnection _db;
		bool _disposed;

		public UnitOfWork()
		{
			_db = Util.CreateDbConnection();			
		}

		public INotesRepository NotesRepository
		{
			get
			{
				if(_notesRepository == null)
				{
					_notesRepository = new NotesRepository(_db);//Kernel.Get<INotesRepository>(new ConstructorArgument("db", _db));
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
					_notebookRepository = new NotebooksRepository(_db);//Kernel.Get<INotebooksRepository>(new ConstructorArgument("db", _db)); ;
				}

				return _notebookRepository;
			}
		}

		public void SaveChanges()
		{
			try
			{
				_db.Commit();


				//used to refresh changes
				_db.Close();
				_notesRepository = null;
				_notebookRepository = null;

				_db = Util.CreateDbConnection();
			}
			catch(Exception e)
			{
				_db.Rollback();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				// Free any other managed objects here.
				_db.Dispose();
			}

			// Free any unmanaged objects here.
			_disposed = true;
		}
	}
}
