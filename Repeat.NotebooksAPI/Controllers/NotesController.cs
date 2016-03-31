using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Repeat.NotebooksAPI.Infrastructure;
using Repeat.NotebooksAPI.Domain.Entities;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Repeat.NotebooksAPI.Controllers
{
	[Route("api/[controller]")]
	public class NotesController : Controller
	{
		IUnitOfWork _unitOfWork;
		JsonSerializerSettings _jsonSerializerSettings;

		public NotesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_jsonSerializerSettings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};
		}

		// GET: api/notes
		[HttpGet]
		public JsonResult Get()
		{
			var notes = _unitOfWork.NotebooksRepository.Get();

			return Json(notes, _jsonSerializerSettings);
		}

		// GET api/notes/5b8567ea-b323-4378-afef-0922eec1892f
		[HttpGet("{id}")]
		public JsonResult Get(Int64 id)
		{
			var note = _unitOfWork.NotesRepository.GetByID(id);

			return Json(note, _jsonSerializerSettings);
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody]Note note)
		{
			_unitOfWork.NotesRepository.Add(note);
			_unitOfWork.Save();
		}

		// PUT api/values/
		[HttpPut("{id}")]
		public void Put(Int64 id, [FromBody]Note note)
		{
			_unitOfWork.NotesRepository.Update(note);
			_unitOfWork.Save();
		}

		// DELETE api/values/5b8567ea-b323-4378-afef-0922eec1892f
		[HttpDelete("{id}")]
		public void Delete(Int64 id)
		{
			_unitOfWork.NotesRepository.Delete(id);
			_unitOfWork.Save();
		}
	}
}
