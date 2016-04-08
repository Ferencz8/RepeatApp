using Repeat.GenericLibs.PCL.APICallers;
using Repeat.SyncronizerService.APICallers.Interfaces;
using Repeat.SyncronizerService.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.APICallers
{
	public class NoteAPICaller : GenericAPICaller<Note>, INoteAPICaller
	{

		public NoteAPICaller()
			: base(ConfigurationManager.AppSettings["NotebookAPI-Address"])
		{ 

		}
	}
}
