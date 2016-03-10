using Repeat.DAL.Entities;
using Repeat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.DAL.Repositories
{
	public class NotebooksRepository : GenericRepository<Notebook>, INotebooksRepository
	{

		public NotebooksRepository(Db db)
			: base(db)
		{

		}
	}
}
