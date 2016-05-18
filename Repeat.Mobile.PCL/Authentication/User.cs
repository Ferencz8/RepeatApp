using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.Authentication
{
	public class User
	{
		public User()
		{
			Permissions = new List<string>();
		}

		public string Id { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public bool EmailVerified { get; set; }

		public string IPAddress { get; set; }

		public DateTime LastLoginAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public DateTime CreatedAt { get; set; }

		public List<string> Permissions { get; set; }

		public string Token { get; set; }
	}
}
