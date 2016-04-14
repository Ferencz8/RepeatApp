//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Repeat.Mobile.PCL.Logging
//{
//	public class Log : ILog
//	{
//		ConcurrentDictionary<string, Guid> _requests;
//		string _tag = "!!!!App";

//		public void AddRequest(string location, Guid requestId)
//		{
//			if (!_requests.ContainsKey(location))
//			{
//				_requests.TryAdd(location, requestId);
//			}
//		}


//		public Guid GetRequest(string location)
//		{
//			if (_requests.ContainsKey(location))
//			{
//				return _requests[location];
//			}
//			return Guid.Empty;
//		}


//		public void Exception(Guid requestId, Exception e, string message = null)
//		{
//			Debug.WriteLine(requestId.ToString() + " " + e.ToString() + " " + message.ToString());
			
//		}

//		public void Info(Guid requestId, string message)
//		{
//			Debug.WriteLine(requestId.ToString() + " " + message.ToString());
//		}

//		public void Warning(Guid requestId, string message)
//		{
//			Debug.WriteLine(requestId.ToString() + " " + message.ToString());
//		}

//		public void Warning(Guid requestId, Exception e, string message = null)
//		{
//			throw new NotImplementedException();
//		}
//	}
//}
