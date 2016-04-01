using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces;
using Repeat.NotebooksAPI.Infrastructure;
using System.Web.Script.Serialization;
using Repeat.NotebooksAPI.Domain.Entities;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Repeat.NotebooksAPI.Controllers
{
	[Route("api/[controller]")]
	public class NotebooksController : Controller
	{

		IUnitOfWork _unitOfWork;
		JsonSerializerSettings _jsonSerializerSettings;

		public NotebooksController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_jsonSerializerSettings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
		}

		// GET: api/notebooks
		[HttpGet]
		public JsonResult Get()
		{
			var notebooks = _unitOfWork.NotebooksRepository.Get();

			return Json(notebooks, _jsonSerializerSettings);
		}

		// GET api/notebooks/5
		[HttpGet("{id}")]
		public JsonResult Get(Int64 id)
		{
			var notebook = _unitOfWork.NotebooksRepository.GetByID(id);

			return Json(notebook, _jsonSerializerSettings);
		}

		// GET api/notebooks/5/notes
		[Route("{notebookId}/notes")]
		[HttpGet("{notebookId}")]
		public JsonResult GetNotes(Int64 notebookId, DateTime? lastSyncDate)
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
		[HttpPost]
		public void Post([FromBody]Notebook notebook)
		{
			_unitOfWork.NotebooksRepository.Add(notebook);
			_unitOfWork.Save();
		}

		// PUT api/notebooks/5b8567ea-b323-4378-afef-0922eec1892f
		[HttpPut("{id}")]
		public void Put(Int64 id, [FromBody]Notebook notebook)
		{
			_unitOfWork.NotebooksRepository.Update(notebook);
			_unitOfWork.Save();
		}

		// DELETE api/notebooks/5b8567ea-b323-4378-afef-0922eec1892f
		[HttpDelete("{id}")]
		public void Delete(Int64 id)
		{
			_unitOfWork.NotebooksRepository.Delete(id);
			_unitOfWork.Save();
		}
	}
}
