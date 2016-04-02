using Newtonsoft.Json;
using Repeat.Mobile.PCL.DAL.Enums;
using SQLite.Net.Attributes;
using System;

namespace Repeat.Mobile.PCL.DAL.Entities
{
	public class Notebook
	{
		[PrimaryKey]
		//[JsonIgnore]
		public Guid Id { get; set; }

		[Unique]
		public string Name { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public SyncStatus SyncStatus { get; set; }
	}
}