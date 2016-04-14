using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.GenericLibs.PCL.APICallers.Interfaces
{
	public interface IGenericAPICaller<T>
	{

		void Add(string apiRoute, T obj);

		Task<List<T>> GetList(string apiRoute);

		Task<T> Get(string apiRoute);

		void Update(string apiRoute, T obj);

		void Delete(string apiRoute, object id);
	}
}
