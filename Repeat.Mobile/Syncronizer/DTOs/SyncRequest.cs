using System;

namespace Repeat.Mobile.Sync.DTOs
{
	public class SyncRequest
	{

		public string UserId { get; set; }
		//TODO:: Device should stand for a specification like ANDROID-12y3yt2ut
		//so that a sync should be made from a certain type of device ANDROID in this case, but it ca also be made
		//from many devices like this one
		public string Device { get { return "ANDROID"; } }

		public string UserToken { get; set; }
	}
}