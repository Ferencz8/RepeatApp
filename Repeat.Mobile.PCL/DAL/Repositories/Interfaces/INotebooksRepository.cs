using Repeat.Mobile.PCL.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repeat.Mobile.PCL.DAL.Repositories.Interfaces
{
	public interface INotebooksRepository : IGenericRepository<Notebook>
	{

		Notebook GetByName(string notebookName);

		List<Notebook> GetNotebooksWithNotesByLastModifiedDateOfNotes(DateTime lastModifiedDate);
	}
}
