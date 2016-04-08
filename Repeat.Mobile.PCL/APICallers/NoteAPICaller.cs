using Repeat.GenericLibs.PCL.APICallers;
using Repeat.Mobile.PCL.APICallers.Interfaces;
using Repeat.Mobile.PCL.Common;
using Repeat.Mobile.PCL.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.APICallers
{
	public class NoteAPICaller : GenericAPICaller<Note>, INoteAPICaller
	{

		public NoteAPICaller()
			: base(Configs.NotebookAPI_Url)
		{

		}
	}
}
