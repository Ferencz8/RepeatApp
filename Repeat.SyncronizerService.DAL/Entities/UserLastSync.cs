using Repeat.SyncronizerService.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DAL.Entities
{
	public class UserLastSync
	{
		[Key, Column(Order = 0)]
		public Guid UserId { get; set; }

		[Key, ForeignKey("Device"), Column(Order = 1)]
		public Guid DeviceId { get; set; }

		public virtual Device Device { get; set; }

		public DateTime LastSyncDate { get; set; }

		public SyncStatus SyncStatus { get; set; }
	}
}
