using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.Repositories
{
	public class DevicesRepository : GenericRepository<Device>, IDevicesRepository
	{

		public DevicesRepository(Db db)
			: base(db)
		{

		}
	}
}
