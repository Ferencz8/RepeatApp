using Newtonsoft.Json;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;

namespace Repeat.Mobile.PCL.DAL.Entities
{
    public class Note
    {
		[PrimaryKey]
		//[JsonIgnore]
		public string Id { get; set; }

		[ForeignKey(typeof(Notebook)), NotNull]
		public string NotebookId { get; set; }

		[OneToMany]
		public Notebook Notebook { get; set; }

		[NotNull]
		public string Name { get; set; }

        public string Content { get; set; }

		[NotNull]
		public DateTime CreatedDate { get; set; }

		[NotNull]
		public DateTime ModifiedDate { get; set; }

		public bool Deleted { get; set; }

		public DateTime? DeletedDate { get; set; }
	}
}