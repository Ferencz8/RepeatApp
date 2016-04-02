using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repeat.GenericLibs.PCL.APICallers;
using Repeat.SyncronizerService.DAL.DTOs;
using Repeat.SyncronizerService.APICallers.Interfaces;

namespace Repeat.SyncronizerService.APICallers
{
	public class NotebookAPICaller : GenericAPICaller<Notebook>, INotebookAPICaller
	{

		public NotebookAPICaller()
			:base("http://www.repeat.somee.com/")
		{

		}

		public Task<List<Note>> GetNotes(string apiRoute, Guid notebookId, DateTime? lastSyncDate = null)
		{
			throw new NotImplementedException();
		}
	}
}