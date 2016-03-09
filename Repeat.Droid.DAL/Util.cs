using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repeat.Droid.DAL
{
	public interface IFile
	{

		bool FileExists(string path);
	}

	public static class Util
	{

		public static IFile File { get; set; }
	}
}
