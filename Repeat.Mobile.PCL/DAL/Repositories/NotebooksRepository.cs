using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.DAL.Repositories
{
	public class NotebooksRepository : GenericRepository<Notebook>, INotebooksRepository
	{
		Db _db;

		public NotebooksRepository(Db db)
			: base(db)
		{
			_db = db;
		}

		public Notebook GetByName(string notebookName)
		{
			return _db.Table<Notebook>().SingleOrDefault(n => n.Name.Equals(notebookName));
		}
	}
}
