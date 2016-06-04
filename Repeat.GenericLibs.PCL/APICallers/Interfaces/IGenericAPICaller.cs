using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.GenericLibs.PCL.APICallers.Interfaces
{
	public interface IGenericAPICaller<T>
	{

		void Add(string apiRoute, T obj, Dictionary<string, string> headers = null);

		Task<List<T>> GetList(string apiRoute, Dictionary<string, string> headers = null);

		Task<T> Get(string apiRoute, Dictionary<string, string> headers = null);

		void Update(string apiRoute, T obj, Dictionary<string, string> headers = null);

		void Delete(string apiRoute, object id, Dictionary<string, string> headers = null);
	}
}
