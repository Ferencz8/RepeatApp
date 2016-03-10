using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Repeat.DAL.Repositories.Interfaces;
using Repeat.DAL.Repositories;
using Repeat.DAL;

namespace Repeat.Droid.DAL.DependencyManagement
{
	public class RepositoryModule : NinjectModule
	{
		public override void Load()
		{
			Bind<INotebooksRepository>().To<NotebooksRepository>();
			Bind<INotesRepository>().To<NotesRepository>();


			//http://stackoverflow.com/questions/25667834/constructor-with-multiple-arguments-with-ninject
			Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope()
				.WithConstructorArgument("sqlLitePlatform", Util.SQLitePlatform)
				.WithConstructorArgument("databasePath", Util.DatabasePath);
		}
	}
}
