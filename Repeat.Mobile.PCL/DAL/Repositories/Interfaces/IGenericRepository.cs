﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.DAL.Repositories.Interfaces
{
	public interface IGenericRepository<T>
		where T : class
	{

		int Add(T obj);

		List<T> Get();

		T GetByID(object id);

		int Update(T obj);

		int Delete(object primaryKey);

		int DeleteAll();
	}
}
