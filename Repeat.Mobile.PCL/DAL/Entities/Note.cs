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
		public Guid Id { get; set; }

		[ForeignKey(typeof(Notebook))]
		public Guid NotebookId { get; set; }

		[OneToOne]
		public Notebook Notebook { get; set; }

		public string Name { get; set; }

        public string Content { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}