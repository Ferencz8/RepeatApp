using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.Entities
{
	public class Device
	{
		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }
	}
}
