using Repeat.NotebooksAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Infrastructure
{
	public class Db : DbContext
	{
		//http://www.entityframeworktutorial.net/EntityFramework4.3/dbcontext-vs-objectcontext.aspx

		public Db()
			:base("Repeat")
		{

		}

		static Db()
		{
			Database.SetInitializer(new DbInitializer());
		}
		
		public IDbSet<Note> Notes { get; set; }

		public IDbSet<Notebook> Notebooks { get; set; }
	}
}
