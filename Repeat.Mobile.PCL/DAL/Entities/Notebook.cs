using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;

namespace Repeat.Mobile.PCL.DAL.Entities
{
	public class Notebook
	{
		[PrimaryKey]
		//[JsonIgnore]
		public string Id { get; set; }

		[Unique, NotNull]
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
		public string UserId { get; set; }
	}
}