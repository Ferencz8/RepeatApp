using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces;
using Repeat.NotebooksAPI.Infrastructure;
using Repeat.NotebooksAPI.Domain.Entities;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Results;

namespace Repeat.NotebooksAPI.Controllers
{
	public class NotebooksController : ApiController
	{

		IUnitOfWork _unitOfWork;
		JsonSerializerSettings _jsonSerializerSettings;

		public NotebooksController()
		{

			_unitOfWork = new UnitOfWork();
			_jsonSerializerSettings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
		}

		// GET: api/notebooks
		 
		public JsonResult<IEnumerable<Notebook>> Get()
		{
			var notebooks = _unitOfWork.NotebooksRepository.Get();

			return Json(notebooks, _jsonSerializerSettings);
		}

		// GET api/notebooks/36A21286-5167-43F1-BC74-1C66471254EA

		public JsonResult<Notebook> Get(Guid id)
		{
			var notebook = _unitOfWork.NotebooksRepository.GetByID(id);
			//temp
			notebook.Notes = new List<Note>();
			return Json(notebook, _jsonSerializerSettings);
		}

		// GET api/notebooks/36A21286-5167-43F1-BC74-1C66471254EA/notes?lastSyncDate=4/7/2016
		[Route("~/api/notebooks/{notebookId:guid}/notes")]
		[HttpGet]
		public JsonResult<List<Note>> Notes(Guid notebookId, [FromUri]DateTime? lastSyncDate)
		{
			List<Note> notes = new List<Note>();

			var notebook = _unitOfWork.NotebooksRepository.GetByID(notebookId);
			if (notebook != null)
			{
				if (!lastSyncDate.HasValue)
				{
					notes = notebook.Notes;
				}
				else
				{
					notes = notebook.Notes.Where(n => n.ModifiedDate > lastSyncDate).ToList();
				}
			}
			return Json(notes, _jsonSerializerSettings);
		}

		// POST api/notebooks
		 
		public void Post([FromBody]Notebook notebook)
		{
			//TODO:: here I should validate the obj
			if(notebook.Id == Guid.Empty)
			{
				notebook.Id = Guid.NewGuid();
				notebook.CreatedDate = DateTime.Now;
				notebook.ModifiedDate = DateTime.Now;
			}
			_unitOfWork.NotebooksRepository.Add(notebook);
			_unitOfWork.Save();
		}

		// PUT api/notebooks/5b8567ea-b323-4378-afef-0922eec1892f
		 
		public void Put(Guid id, [FromBody]Notebook notebook)
		{
			notebook.Id = id;
			_unitOfWork.NotebooksRepository.Update(notebook);
			_unitOfWork.Save();
		}

		// DELETE api/notebooks/5b8567ea-b323-4378-afef-0922eec1892f
		 
		public void Delete(Guid id)
		{
			_unitOfWork.NotebooksRepository.Delete(id);
			_unitOfWork.Save();
		}
	}
}
