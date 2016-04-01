using Repeat.SyncronizerService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL
{
	public interface IUnitOfWork
	{

		IDevicesRepository DevicesRepository { get; }

		IUsersLastSyncRepository UsersLastSyncRepository { get; }

		void Save();
	}
}
