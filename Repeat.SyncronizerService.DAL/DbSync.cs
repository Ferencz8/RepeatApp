using Repeat.SyncronizerService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL
{
	public class DbSync : DbContext
	{

		public DbSync()
			: base("name=Db")
		{

		}

		static DbSync()
		{
			Database.SetInitializer(new DbInitializer());
		}

		
		public IDbSet<Device> Devices { get; set; }

		public IDbSet<UserLastSync> UsersLastSync { get; set; }
	}
}
