using Repeat.SyncronizerService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL
{
	public class Db : DbContext
	{

		public Db()
			: base("Sync")
		{

		}

		static Db()
		{
			Database.SetInitializer(new DbInitializer());
		}

		
		public IDbSet<Device> Devices { get; set; }

		public IDbSet<UserLastSync> UsersLastSync { get; set; }
	}
}
