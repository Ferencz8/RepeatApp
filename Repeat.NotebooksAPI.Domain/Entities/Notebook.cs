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
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64 Id { get; set; }//TODO:: check if it is or not better to use a key of type int


		[StringLength(100)]
		[Index(IsUnique = true)]
		public string Name { get; set; }

		public virtual List<Note> Notes { get; set; }
		
		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public bool Deleted { get; set; }

		public DateTime DeletedDate { get; set; }
	}
}
