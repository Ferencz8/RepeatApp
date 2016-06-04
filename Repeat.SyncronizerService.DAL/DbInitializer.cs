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
	public class DbInitializer : CreateDatabaseIfNotExists<DbSync>
	{
		public DbInitializer() : base()
		{

		}

		protected override void Seed(DbSync context)
		{
			base.Seed(context);

			Initialize(context);
		}

		private void Initialize(DbSync context)
		{


			Device device = new Device()
			{
				Id = Guid.NewGuid(),
				Name = "ANDROID",
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

						//logging
					}
				}
			}
		}
	}
}
