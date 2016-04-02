using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.NotebooksAPI.Domain.Entities
{

	//https://stackoverflow.com/questions/262547/reasons-not-to-use-an-auto-incrementing-number-for-a-primary-key
	//https://stackoverflow.com/questions/932102/what-data-type-is-recommended-for-id-columns
	public class Notebook
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

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
