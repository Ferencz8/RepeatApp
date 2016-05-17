using System.Threading.Tasks;
using System.Collections.Generic;
using Repeat.GenericLibs.PCL.APICallers.Interfaces;
using Repeat.SyncronizerService.DTOs;
using System;

namespace Repeat.SyncronizerService.APICallers.Interfaces
{
	public interface INotebookAPICaller : IGenericAPICaller<Notebook>
	{

		Task<List<Note>> GetNotes(string apiRoute, Guid notebookId, DateTime? lastSyncDate = null);
	}
}