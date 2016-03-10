using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repeat.DAL
{
	public interface IFile
	{

		bool FileExists(string path);
	}

	public static class Util
	{

		public static IFile File { get; set; }

		public static ISQLitePlatform SQLitePlatform { get; set; }

		public static string DatabasePath { get; set; }
	}
}
