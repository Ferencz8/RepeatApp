using Repeat.Mobile.PCL.DAL;
using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using Repeat.Mobile.PCL;
using Repeat.Mobile.PCL.Common;

namespace Repeat.Mobile
{
	public class StartUp
	{

		public static void Configure()
		{
			SetUpDatabase();
		}

		private static void SetUpDatabase()
		{
			string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), Configs.DatabaseName);
			Util.CreateConnection(new SQLitePlatformAndroid(), path);
		}
	}
}