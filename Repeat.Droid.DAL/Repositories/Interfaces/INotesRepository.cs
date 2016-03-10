using Repeat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.DAL.Repositories.Interfaces
{
	public interface INotesRepository : IGenericRepository<Note>
	{

		List<Note> GetNotesByNotebookId(int notebookId);
	}
}
