﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DTOs
{
	public class Notebook : Prototype<Notebook>
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public virtual List<Note> Notes { get; set; }
		
		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public bool Deleted { get; set; }

		public DateTime? DeletedDate { get; set; }

		public string UserId { get; set; }
	}
}
