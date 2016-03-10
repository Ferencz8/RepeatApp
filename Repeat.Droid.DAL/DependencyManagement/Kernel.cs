﻿using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Droid.DAL.DependencyManagement
{
	public static class Kernel
	{
		static StandardKernel _container;

		static Kernel()
		{
			_container = new StandardKernel(GetModules());
		}

		public static T Get<T>(params IParameter[] parameters)
		{
			return _container.Get<T>(parameters);
		}

		private static INinjectModule[] GetModules()
		{
			return new INinjectModule[]
			{
				new RepositoryModule(),
			};
		}
	}
}
