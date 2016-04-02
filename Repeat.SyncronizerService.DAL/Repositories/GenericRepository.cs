using Repeat.SyncronizerService.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.Repositories
{
	//TODO:: externalize this class <- this is duplicate code
	public class GenericRepository<TEntity> : IGenericRepository<TEntity>
		where TEntity : class
	{
		//http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
		//http://www.codeproject.com/Articles/814768/CRUD-Operations-Using-the-Generic-Repository-Patte
		Db _db;
		DbSet<TEntity> _dbSet;

		public GenericRepository(Db db)
		{
			_db = db;
			_dbSet = _db.Set<TEntity>();
		}

		public virtual IEnumerable<TEntity> Get(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "")
		{
			IQueryable<TEntity> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			foreach (var includeProperty in includeProperties.Split
				(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}

			if (orderBy != null)
			{
				return orderBy(query).ToList();
			}
			else
			{
				return query.ToList();
			}
		}

		public virtual TEntity GetByID(object id)
		{
			return _dbSet.Find(id);
		}

		public virtual void Add(TEntity entity)
		{
			_dbSet.Add(entity);
		}

		public virtual void Delete(object id)
		{
			TEntity entityToDelete = _dbSet.Find(id);
			Delete(entityToDelete);
		}

		public virtual void Delete(TEntity entityToDelete)
		{
			if (_db.Entry(entityToDelete).State == EntityState.Detached)
			{
				_dbSet.Attach(entityToDelete);
			}
			_dbSet.Remove(entityToDelete);
		}

		public virtual void Update(TEntity entityToUpdate)
		{
			_dbSet.Attach(entityToUpdate);
			_db.Entry(entityToUpdate).State = EntityState.Modified;
		}

		public void DeleteAll()
		{
			_dbSet.RemoveRange(Get());
		}
	}
}