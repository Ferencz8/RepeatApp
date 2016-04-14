using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService
{
	public static class Log
	{
		private static Object _locker = new object();

		public static void Info(string message)
		{
			lock (_locker)
			{
				Console.WriteLine(message + "\n");
			}
		}
	}
}
