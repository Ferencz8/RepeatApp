using Repeat.SyncronizerService.DAL.Repositories;
using Repeat.SyncronizerService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL
{
	public class UnitOfWork : IUnitOfWork
	{
		bool _disposed;
		DbSync _db = new DbSync();
		IDevicesRepository _devicesRepository;
		IUsersLastSyncRepository _usersLastSyncRepository;


		public IDevicesRepository DevicesRepository
		{
			get
			{
				if (_devicesRepository == null)
				{
					_devicesRepository = new DevicesRepository(_db);
				}

				return _devicesRepository;
			}
		}

		public IUsersLastSyncRepository UsersLastSyncRepository
		{
			get
			{
				if (_usersLastSyncRepository == null)
				{
					_usersLastSyncRepository = new UsersLastSyncRepository(_db);
				}
				return _usersLastSyncRepository;
			}
		}

		public void Save()
		{
			_db.SaveChanges();
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
				_db.Database.Connection.Dispose();
			}

			// Free any unmanaged objects here.
			_disposed = true;
		}
	}
}

