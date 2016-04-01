using Repeat.SyncronizerService.APICallers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.APICallers
{
	public class CentralAPICaller
	{

		private INoteAPICaller _noteAPICaller;
		private INotebookAPICaller _notebookAPICaller;

		public INoteAPICaller NoteAPICaller
		{
			get
			{
				if(_noteAPICaller == null)
				{
					_noteAPICaller = new NoteAPICaller();
				}
				return _noteAPICaller;
			}
		}

		public INotebookAPICaller NotebookAPICaller
		{
			get
			{
				if (_notebookAPICaller == null)
				{
					_notebookAPICaller = new NotebookAPICaller();
				}
				return _notebookAPICaller;
			}
		}
	}
}
