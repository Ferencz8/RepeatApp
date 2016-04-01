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

		Db _db = new Db();
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
	}
}

