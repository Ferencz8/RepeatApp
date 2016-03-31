using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.APICallers.Interfaces
{
	public interface IGenericAPICaller<T>
	{

		void Add(string apiRoute, T obj);

		Task<List<T>> Get(string apiRoute);

		Task<T> GetById(string apiRoute);

		void Update(string apiRoute, T obj);

		void Delete(string apiRoute, object id);
	}
}
