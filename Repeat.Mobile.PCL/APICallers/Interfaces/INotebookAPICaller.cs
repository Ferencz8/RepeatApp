using Repeat.Mobile.PCL.DAL.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Repeat.GenericLibs.PCL.APICallers.Interfaces;

namespace Repeat.Mobile.PCL.APICallers.Interfaces
{
	public interface INotebookAPICaller : IGenericAPICaller<Notebook>
	{

		Task<List<Note>> GetNotes(string apiRoute);
	}
}