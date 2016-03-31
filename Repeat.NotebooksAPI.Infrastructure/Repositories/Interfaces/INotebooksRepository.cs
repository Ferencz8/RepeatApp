using Repeat.NotebooksAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces
{
	public interface INotebooksRepository : IGenericRepository<Notebook>
	{
	}
}
