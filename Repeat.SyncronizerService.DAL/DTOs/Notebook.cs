using Repeat.SyncronizerService.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.DTOs
{
	public class Notebook
	{
		public Int64 Id { get; set; }

		public string Name { get; set; }

		public virtual List<Note> Notes { get; set; }
		
		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public bool Deleted { get; set; }

		public DateTime DeletedDate { get; set; }
	}
}
