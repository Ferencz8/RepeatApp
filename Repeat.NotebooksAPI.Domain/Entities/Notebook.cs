using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Domain.Entities
{
	public class Notebook
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[StringLength(100)]
		[Index(IsUnique = true)]
		public string Name { get; set; }

		public virtual List<Note> Notes { get; set; }

		[Required]
		public DateTime CreatedDate { get; set; }

		[Required]
		public DateTime ModifiedDate { get; set; }

		public bool Deleted { get; set; }

		public DateTime? DeletedDate { get; set; }
	}
}
