using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Repeat.Mobile.PCL.Common;
using Repeat.Mobile.PCL.DAL.Entities;
using Repeat.Mobile.PCL.APICallers;
using Repeat.Mobile.PCL.APICallers.Interfaces;

namespace Repeat.Mobile.PCL.APICallers
{
	public class NotebookAPICaller : GenericAPICaller<Notebook>, INotebookAPICaller
	{

		public Task<List<Note>> GetNotes(string apiRoute)
		{
			throw new NotImplementedException();
		}
	}
}