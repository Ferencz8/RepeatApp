using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DTOs
{
	public class SyncResult : SyncRequest
	{
		public bool Result { get; set; }

		public new DataToBeSynched Clone()
		{
			return base.Clone<DataToBeSynched>();
		}
	}
}