using Repeat.NotebooksAPI.Domain.Entities;
using Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Infrastructure.Repositories
{
	public class NotesRepository : GenericRepository<Note>, INotesRepository
	{

		public NotesRepository(Db db)
			: base(db)
		{

		}
	}
}
