﻿using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.DAL.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T>
		where T : class
	{
		SQLiteConnection _db;

		public GenericRepository(SQLiteConnection db)
		{
			_db = db;
			if (!_db.IsInTransaction)
			{
				_db.BeginTransaction();
			}
		}

		public virtual int Add(T obj)
		{
			return _db.Insert(obj);
		}
		
		public virtual List<T> Get()
		{
			return _db.Table<T>().ToList();
		}

		public virtual T GetByID(object id)
		{
			return _db.Find<T>(id);
		}

		public virtual int Update(T obj)
		{
			return _db.Update(obj);
		}

		public virtual int Delete(object objPrimaryKey)
		{
			return _db.Delete<T>(objPrimaryKey);
		}

		public virtual int DeleteAll()
		{
			return _db.DeleteAll<T>();
		}
	}
}
