using Repeat.SyncronizerService.DTOs;
using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.Repositories
{
	public class UsersLastSyncRepository : GenericRepository<UserLastSync>, IUsersLastSyncRepository
	{
		private DbSync _db;

		public UsersLastSyncRepository(DbSync db)
			: base(db)
		{
			_db = db;
		}

		public UserLastSync GetUserLastSyncFor(SyncRequest message)
		{
			return _db.UsersLastSync.FirstOrDefault(n => n.UserId.Equals(message.UserId) && n.Device.Name.Equals(message.Device));
		}
	}
}
