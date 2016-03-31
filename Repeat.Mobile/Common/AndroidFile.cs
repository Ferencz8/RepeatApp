using System.IO;

namespace Repeat.Mobile.Common
{
	public class AndroidFile
	{
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}
	}
}