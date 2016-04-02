﻿using Repeat.SyncronizerService.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.DTOs
{
	public class Note
	{
		public Guid Id { get; set; }

		public Guid NotebookId { get; set; }
		
		public Notebook Notebook { get; set; }

		public string Name { get; set; }

		public string Content { get; set; }
		
		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public SyncStatus SyncStatus { get; set; }

		public bool Deleted { get; set; }

		public DateTime DeletedDate { get; set; }
	}
}
