//using System;
//using System.Collections.Generic;
//using Repeat.PCL.DAL.Entities;
//using Repeat.PCL.DAL.Repositories.Interfaces;
//using System.Threading.Tasks;
//using Repeat.PCL.DependencyManagement;
//using Repeat.PCL.Common;
//using Repeat.PCL.APICallers.Interfaces;

//namespace Repeat.Synchronizer
//{
//	public class Synchronizer
//	{
//		public async void StartSync()
//		{
//			await SyncNotebooks();

//			await SyncNotes();
//		}

//		private async Task SyncNotes()
//		{
//			throw new NotImplementedException();
//		}

//		private async Task SyncNotebooks()
//		{
//			List<Notebook> apiNotebooks = await Kernel.Get<INotebookAPICaller>().Get(APIRoutes.Notebooks_Get);
//			List<Notebook> apiNotebooksToBeUpdated = new List<Notebook>();

//			//here only the notebooks should be trated without their notes
//			INotebooksRepository notebooksRepository = Kernel.Get<INotebooksRepository>();
//			foreach (var apiNotebook in apiNotebooks)
//			{
//				Notebook notebookDb = notebooksRepository.GetByID(apiNotebook.Id);
//				if (notebookDb == null || notebookDb.ModifiedDate < apiNotebook.ModifiedDate)
//				{
//					//if notebookDb is null -> in this db the notebook object does not exist -> we insert it
//					//if notebookDb.ModifiedDate <= notebook.ModifiedDate -> the notebook from the API is more recent -> so we delete the old record and insert the recent one
//					notebooksRepository.Delete(notebookDb);
//					notebooksRepository.Add(apiNotebook);
//				}
//				else if (notebookDb.ModifiedDate > apiNotebook.ModifiedDate)
//				{
//					//the API should be updated
//					apiNotebooksToBeUpdated.Add(notebookDb);
//				}
//			}

//			//at the end apiNotebooksToBeUpdated should be used to update records from API
//		}


//		//TODO:: implement dates for when modification were made to know which is more accurate the web or mobile version
//		//public async void StartSync()
//		//{
//		//	try
//		//	{
//		//		var client = HttpClientExtensions.GetClient();
//		//		var response = await client.GetAsync("api/notebooks");
//		//		if (response != null)
//		//		{
//		//			string str = await response.Content.ReadAsStringAsync();
//		//			List<Notebook> apiNotebooks = JsonConvert.DeserializeObject<List<Notebook>>(str);
//		//			List<Notebook> apiNotebooksToBeUpdated = new List<Notebook>();

//		//			//here only the notebooks should be trated without their notes
//		//			INotebooksRepository notebooksRepository = Kernel.Get<INotebooksRepository>();
//		//			foreach (var apiNotebook in apiNotebooks)
//		//			{
//		//				Notebook notebookDb = notebooksRepository.GetByID(apiNotebook.Id);
//		//				if (notebookDb == null || notebookDb.ModifiedDate < apiNotebook.ModifiedDate)
//		//				{
//		//					//if notebookDb is null -> in this db the notebook object does not exist -> we insert it
//		//					//if notebookDb.ModifiedDate <= notebook.ModifiedDate -> the notebook from the API is more recent -> so we delete the old record and insert the recent one
//		//					notebooksRepository.Delete(notebookDb);
//		//					notebooksRepository.Add(apiNotebook);
//		//				}
//		//				else if(notebookDb.ModifiedDate > apiNotebook.ModifiedDate)
//		//				{
//		//					//the API should be updated
//		//					apiNotebooksToBeUpdated.Add(notebookDb);
//		//				}
//		//			}

//		//			//at the end apiNotebooksToBeUpdated should be used to update records from API
//		//		}
//		//		else
//		//		{

//		//		}
//		//	}
//		//	catch (Exception e)
//		//	{

//		//	}
//		//}
//	}
//}