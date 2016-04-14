using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repeat.GenericLibs.PCL.APICallers;
using Repeat.SyncronizerService.DTOs;
using Repeat.SyncronizerService.APICallers.Interfaces;
using Newtonsoft.Json;
using System.Configuration;

namespace Repeat.SyncronizerService.APICallers
{
	public class NotebookAPICaller : GenericAPICaller<Notebook>, INotebookAPICaller
	{

		public NotebookAPICaller()
			:base(ConfigurationManager.AppSettings["NotebookAPI-Address"])
		{
		}

		public async Task<List<Note>> GetNotes(string apiRoute, Guid notebookId, DateTime? lastSyncDate = null)
		{
			List<Note> elements = new List<Note>();
			try
			{
				var client = HttpClientExtensions.GetAPIClient(_apiURL);
				string date = !lastSyncDate.HasValue ? string.Empty : "?lastSyncDate=" + lastSyncDate.Value.ToShortDateString();

				var response = await client.GetAsync(_apiURL + string.Format(apiRoute, notebookId, lastSyncDate));
				if (response != null)
				{
					string str = await response.Content.ReadAsStringAsync();
					elements = JsonConvert.DeserializeObject<List<Note>>(str, _settings);
				}
			}
			catch (Exception e)
			{
				//TODO LOG
			}
			return elements;
		}
	}
}