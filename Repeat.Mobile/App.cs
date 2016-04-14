using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using Repeat.Mobile.PCL;
using Repeat.Mobile.PCL.Common;
using System.Threading.Tasks;
using System;
using Android.Util;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.Logging;

namespace Repeat.Mobile
{
	public class App
	{
		private static App _current;

		static App()
		{
			_current = new App();
		}

		public static App Current { get { return _current; } }

		protected App()
		{

			// subscribe to app wide unhandled exceptions so that we can log them.
			AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

			new Task(() =>
			{
				Util.Log = new Logger();

				SetUpDatabase();
			}).Start();
		}

		private void SetUpDatabase()
		{
			string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), Configs.DatabaseName);
			Util.CreateDbConnection(new SQLitePlatformAndroid(), path);
		}


		protected void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;

			// log won't be available, because dalvik is destroying the process
			//Log.Debug (logTag, "MyHandler caught : " + e.Message);
			// instead, your err handling code shoudl be run:
			Kernel.Get<ILog>().Exception(Guid.Empty, e, "Global Exception Handler");
			Console.WriteLine("========= MyHandler caught : " + e.Message);
		}
	}
}