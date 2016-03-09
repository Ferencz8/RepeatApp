using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Repeat.Droid.DAL;
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