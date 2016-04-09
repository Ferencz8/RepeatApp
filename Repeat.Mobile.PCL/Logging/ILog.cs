using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.Mobile.PCL.Logging
{
	public interface ILog
	{

		void Exception(Guid requestId, Exception e, string message = null);

		void Info(Guid requestId, string message);

		void Warning(Guid requestId, Exception e, string message = null);

		void Warning(Guid requestId, string message);

		Guid GetRequest(string location);

		void AddRequest(string location, Guid requestId);
	}
}
