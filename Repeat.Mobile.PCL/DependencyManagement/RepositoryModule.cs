using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Repeat.Mobile.PCL.DAL.Repositories.Interfaces;
using Repeat.Mobile.PCL.DAL.Repositories;
using Repeat.Mobile.PCL.DAL;

namespace Repeat.Mobile.PCL.DependencyManagement
{
	public class RepositoryModule : NinjectModule
	{
		public override void Load()
		{
			var db = new Db(Util.SQLitePlatform, Util.DatabasePath);

			Bind<INotebooksRepository>().To<NotebooksRepository>().WithConstructorArgument("db", db);
			Bind<INotesRepository>().To<NotesRepository>().WithConstructorArgument("db", db);


			//http://stackoverflow.com/questions/25667834/constructor-with-multiple-arguments-with-ninject
			//Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope()
			//	.WithConstructorArgument("sqlLitePlatform", Util.SQLitePlatform)
			//	.WithConstructorArgument("databasePath", Util.DatabasePath);
		}
	}
}
