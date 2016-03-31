using Ninject.Modules;
using Repeat.Mobile.PCL.APICallers.Interfaces;
using Repeat.Mobile.PCL.APICallers;

namespace Repeat.Mobile.PCL.DAL.DependencyManagement
{
	public class APICallerModule : NinjectModule
	{

		public override void Load()
		{
			Bind<INoteAPICaller>().To<NoteAPICaller>();
			Bind<INotebookAPICaller>().To<NotebookAPICaller>();
		}
	}
}
