using System.Threading.Tasks;
using System.Collections.Generic;
using Repeat.GenericLibs.PCL.APICallers.Interfaces;
using Repeat.SyncronizerService.DAL.DTOs;
using System;

namespace Repeat.SyncronizerService.APICallers.Interfaces
{
	public interface INotebookAPICaller : IGenericAPICaller<Notebook>
	{

		Task<List<Note>> GetNotes(string apiRoute, Int64 notebookId, DateTime? lastSyncDate = null);
	}
}