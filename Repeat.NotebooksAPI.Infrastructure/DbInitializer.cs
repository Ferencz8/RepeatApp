using Repeat.NotebooksAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Infrastructure
{
	public class DbInitializer : CreateDatabaseIfNotExists<Db>
	{

		public DbInitializer() : base()
		{

		}

		protected override void Seed(Db context)
		{
			base.Seed(context);

			Initialize(context);
		}

		private void Initialize(Db context)
		{
			Notebook notebook = new Notebook()
			{
				Id = 12,
				Name = "First Notebook",
			};

			context.Notebooks.Add(notebook);

			try
			{
				context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{

						//TODO:: implement logging
					}
				}
			}		
		}
	}
}
