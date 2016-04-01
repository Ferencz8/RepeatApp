using Repeat.SyncronizerService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL
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
			Device device = new Device()
			{
				Id = Guid.NewGuid(),
				Name = "Android",
			};

			context.Devices.Add(device);

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
