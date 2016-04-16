using System;
using Repeat.NotebooksAPI.Infrastructure;
using Repeat.NotebooksAPI.Domain.Entities;
using Newtonsoft.Json;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Results;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Repeat.NotebooksAPI.Controllers
{
	public class NotesController : ApiController
	{
		IUnitOfWork _unitOfWork;
		JsonSerializerSettings _jsonSerializerSettings;
		public NotesController()
		{
			_unitOfWork = new UnitOfWork();
			_jsonSerializerSettings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
		}

		// GET: api/notes
		[HttpGet]
		public JsonResult<IEnumerable<Note>> GetNotes()
		{
			var notes = _unitOfWork.NotesRepository.Get();

			return Json(notes, _jsonSerializerSettings);
		}

		// GET api/notes/5b8567ea-b323-4378-afef-0922eec1892f
		public JsonResult<Note> Get(Guid id)
		{
			var note = _unitOfWork.NotesRepository.GetByID(id);

			return Json(note, _jsonSerializerSettings);
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody]Note note)
		{
			if (note.Id == Guid.Empty)
			{
				note.Id = Guid.NewGuid();
				note.CreatedDate = DateTime.UtcNow;
				note.ModifiedDate = DateTime.UtcNow;
			}
			_unitOfWork.NotesRepository.Add(note);
			_unitOfWork.Save();
		}

		// PUT api/values/
		public void Put(Guid id, [FromBody]Note note)
		{
			note.Id = id;
			_unitOfWork.NotesRepository.Update(note);
			_unitOfWork.Save();
		}

		// DELETE api/values/5b8567ea-b323-4378-afef-0922eec1892f
		public void Delete(Guid id)
		{
			_unitOfWork.NotesRepository.Delete(id);
			_unitOfWork.Save();
		}
	}
}
