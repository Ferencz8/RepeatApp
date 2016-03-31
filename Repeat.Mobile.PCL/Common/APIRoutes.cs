using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.Common
{
	public class APIRoutes
	{

		public const string Notebooks_Get = "api/notebooks";

		public const string Notebooks_GetByID = "api/notebooks/{0}";

		public const string Notebooks_GetNotes = "api/notebooks/{0}/notes";

		public const string Notebooks_Post = "api/notebooks";

		public const string Notebooks_Put = "api/notebooks/{0}";

		public const string Notebooks_Delete = "api/notebooks/{0}";
	}
}
