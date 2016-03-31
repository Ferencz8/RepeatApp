using Repeat.Mobile.PCL.DAL;
using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using Repeat.Mobile.PCL;

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
			string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "repeatDb.db3");
			Util.SQLitePlatform = new SQLitePlatformAndroid();
			Util.DatabasePath = path;
		}
	}
}