using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Infrastructure.Repositories.Interfaces
{
	public interface IGenericRepository<TEntity>
		where TEntity : class
	{

		IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "");

		TEntity GetByID(object id);

		void Add(TEntity entity);

		void Update(TEntity entity);

		void Delete(object id);

		void Delete(TEntity entity);

		void DeleteAll();
	}
}
