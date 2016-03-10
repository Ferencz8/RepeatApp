using Repeat.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.DAL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T>
		where T : class
	{
		Db _db;

		public GenericRepository(Db db)
		{
			_db = db;
		}

		public int Add(T obj)
		{
			return _db.Insert(obj);
		}
		
		public List<T> Get()
		{
			return _db.Table<T>().ToList();
		}

		public int Update(T obj)
		{
			return _db.Update(obj);
		}

		public int Delete(object objPrimaryKey)
		{
			return _db.Delete<T>(objPrimaryKey);
		}

		public int DeleteAll()
		{
			return _db.DeleteAll<T>();
		}
	}
}
