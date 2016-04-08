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

			UserLastSync user = new UserLastSync()
			{
				UserId = Guid.Empty,
				Device = device,
				LastSyncDate = DateTime.Now.AddDays(-30),
				SyncStatus = Enums.SyncStatus.Stopped,
			};

			context.Devices.Add(device);
			context.UsersLastSync.Add(user);
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
