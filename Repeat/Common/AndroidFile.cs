using System.IO;

namespace Repeat.DAL
{
	public class AndroidFile : IFile
	{
		public bool FileExists(string path)
		{
			return File.Exists(path);
		}
	}
}