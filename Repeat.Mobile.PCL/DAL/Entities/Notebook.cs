using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace Repeat.Mobile.PCL.DAL.Entities
{
	public class Notebook
	{
		public Notebook()
		{
			Notes = new List<Note>();
		}

		[PrimaryKey]
		//[JsonIgnore]
		public string Id { get; set; }

		//[Unique, NotNull]
		[NotNull]
		[Indexed(Name = "IX_NameAndUser", Order = 1, Unique = true)]
		public string Name { get; set; }

		[NotNull]
		public DateTime CreatedDate { get; set; }

		[NotNull]
		public DateTime ModifiedDate { get; set; }

		public bool Deleted { get; set; }

		public DateTime? DeletedDate { get; set; }

		[OneToMany()]
		public virtual List<Note> Notes { get; set; }

		[NotNull]
		[Indexed(Name = "IX_NameAndUser", Order = 2, Unique = true)]
		public string UserId { get; set; }
	}
}