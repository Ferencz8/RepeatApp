using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DTOs
{
	public class SyncRequest : Prototype<SyncRequest>
	{

		public Guid UserId { get; set; }

		public string Device { get; set; }
	}
}
