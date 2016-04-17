using Repeat.NotebooksAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Domain.Entities
{
	public class Note
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public Guid Id { get; set; }

		[Required]
		public Guid NotebookId { get; set; }

		[ForeignKey("NotebookId")]
		public Notebook Notebook { get; set; }

		[Required]
		public string Name { get; set; }

		public string Content { get; set; }

		[Required]
		public DateTime CreatedDate { get; set; }

		[Required]
		public DateTime ModifiedDate { get; set; }

		public bool Deleted { get; set; }

		public DateTime? DeletedDate { get; set; }
	}
}
