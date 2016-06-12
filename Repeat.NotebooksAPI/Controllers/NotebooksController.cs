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
using Repeat.NotebooksAPI.ActionFilters;

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
		// GET: api/notebooks/?deleted=true&userId=wnvksandkavb
		//[AuthorizationRequired]
		public JsonResult<IEnumerable<Notebook>> Get(bool? deleted = null, string userId = null)
		{
			IEnumerable<Notebook> notebooks = new List<Notebook>();
			if (deleted.HasValue && !string.IsNullOrEmpty(userId))
			{
				notebooks = _unitOfWork.NotebooksRepository.Get(n => n.Deleted.Equals(deleted.Value) && n.UserId.Equals(userId));
			}
			else if (deleted.HasValue)
			{
				notebooks = _unitOfWork.NotebooksRepository.Get(n => n.Deleted.Equals(deleted.Value));
			}
			else if (!string.IsNullOrEmpty(userId))
			{
				notebooks = _unitOfWork.NotebooksRepository.Get(n => n.UserId.Equals(userId));
			}
			else {
				notebooks = _unitOfWork.NotebooksRepository.Get();
			}

			return Json(notebooks, _jsonSerializerSettings);
		}

		// GET api/notebooks/36A21286-5167-43F1-BC74-1C66471254EA

		public JsonResult<Notebook> Get(Guid id)
		{
			var notebook = _unitOfWork.NotebooksRepository.GetByID(id);
			//temp
			if (notebook != null)
			{ notebook.Notes = new List<Note>(); }
			return Json(notebook, _jsonSerializerSettings);
		}

		// GET api/notebooks/36A21286-5167-43F1-BC74-1C66471254EA/notes
		// GET api/notebooks/36A21286-5167-43F1-BC74-1C66471254EA/notes?lastSyncDate=4/7/2016&deleted=false
		[Route("~/api/notebooks/{notebookId:guid}/notes")]
		[HttpGet]
		public JsonResult<List<Note>> Notes(Guid notebookId, [FromUri]DateTime? lastSyncDate = null, bool? deleted = null)
		{
			List<Note> notes = new List<Note>();

			var notebook = _unitOfWork.NotebooksRepository.GetByID(notebookId);
			if (notebook != null)
			{
				if (lastSyncDate.HasValue && deleted.HasValue)
				{
					notes = notebook.Notes.Where(n => n.ModifiedDate > lastSyncDate && n.Deleted.Equals(deleted)).ToList();
				}
				else if (lastSyncDate.HasValue)
				{
					notes = notebook.Notes.Where(n => n.ModifiedDate > lastSyncDate).ToList();
				}
				else if (deleted.HasValue)
				{
					notes = notebook.Notes.Where(n => n.Deleted.Equals(deleted)).ToList();
				}
				else
				{
					notes = notebook.Notes;
				}
			}
			return Json(notes, _jsonSerializerSettings);
		}

		// POST api/notebooks

		public void Post([FromBody]Notebook notebook)
		{
			//TODO:: check if notes are inserted
			if (notebook.Id == Guid.Empty)
			{
				notebook.Id = Guid.NewGuid();
				notebook.CreatedDate = DateTime.UtcNow;
				notebook.ModifiedDate = DateTime.UtcNow;
			}
			_unitOfWork.NotebooksRepository.Add(notebook);
			_unitOfWork.Save();
		}

		// PUT api/notebooks/5b8567ea-b323-4378-afef-0922eec1892f

		public void Put(Guid id, [FromBody]Notebook notebook)
		{
			notebook.Id = id;
			if (notebook.Deleted)
			{
				notebook.Notes.ForEach(n =>
				{
					n.Deleted = true;
					n.DeletedDate = DateTime.UtcNow;
					n.ModifiedDate = DateTime.UtcNow;
				});
			}

			_unitOfWork.NotebooksRepository.Update(notebook);
			_unitOfWork.Save();
		}

		//// DELETE api/notebooks/5b8567ea-b323-4378-afef-0922eec1892f

		//public void Delete(Guid id)
		//{
		//	_unitOfWork.NotebooksRepository.Delete(id);
		//	_unitOfWork.Save();
		//}
	}
}
