using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.DAL
{
	public interface IUnitOfWork
	{

		INotesRepository NotesRepository { get; }

		INotebooksRepository NotebooksRepository { get; }
	}
}
