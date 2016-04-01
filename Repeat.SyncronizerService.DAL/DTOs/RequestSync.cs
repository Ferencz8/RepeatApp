using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.DTOs
{
	public class RequestSync
	{

		public Guid UserId { get; set; }

		public string Device { get; set; }
	}
}
