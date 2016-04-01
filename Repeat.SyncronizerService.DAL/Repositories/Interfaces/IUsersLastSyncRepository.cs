using Repeat.SyncronizerService.DAL.DTOs;
using Repeat.SyncronizerService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.Repositories.Interfaces
{
	public interface IUsersLastSyncRepository : IGenericRepository<UserLastSync>
	{
		UserLastSync GetUserLastSyncFor(RequestSync message);
	}
}
