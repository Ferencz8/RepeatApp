using Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Infrastructure
{
	public interface IUnitOfWork
	{

		INotebooksRepository NotebooksRepository { get; }

		INotesRepository NotesRepository { get; }

		void Save();
	}
}
