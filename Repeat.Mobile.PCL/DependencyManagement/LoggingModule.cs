﻿using Ninject.Modules;
using Repeat.Mobile.PCL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.DependencyManagement
{
	public class LoggingModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ILog>().To(Util.Log.GetType());
		}
	}
}
