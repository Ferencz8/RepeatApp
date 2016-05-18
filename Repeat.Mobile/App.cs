using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using Repeat.Mobile.PCL;
using Repeat.Mobile.PCL.Common;
using System;
using Repeat.Mobile.PCL.DependencyManagement;
using Repeat.Mobile.PCL.Logging;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Repeat.Mobile
{
	public class App
	{
		public event EventHandler<EventArgs> Initialized = delegate { };

		public bool IsInitialized { get; set; }


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
			//new Task(() =>
			//{
			Util.Log = new Logger();

			//Kernel.Get<ILog>().Info(Guid.Empty, "Init started!");

			SetUpDatabase();

			// set our initialization flag so we know that we're all setup
			this.IsInitialized = true;
			// raise our intialized event
			this.Initialized(this, new EventArgs());

			Kernel.Get<ILog>().Info(Guid.Empty, "Init finished!");
			//}).Start();
		}

		private void SetUpDatabase()
		{
			Util.DatabasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), Configs.DatabaseName);
			Util.SQLitePlatform = new SQLitePlatformAndroid();
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