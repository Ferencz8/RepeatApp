using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.Common
{
	public class Config
	{

		public static string RabbitMQUsername = ConfigurationManager.AppSettings["RabbitMQ-Username"];

		public static string RabbitMQPassword = ConfigurationManager.AppSettings["RabbitMQ-Password"];

		public static string RabbitMQHostName;

		public static string RabbitMQPort = ConfigurationManager.AppSettings["RabbitMQ-Port"];

		public static string RabbitMQVirtualHost = ConfigurationManager.AppSettings["RabbitMQ-VirtualHost"];

		public static string RabbitMQ_DataToBeSynchedQueue = ConfigurationManager.AppSettings["RabbitMQ-DataToBeSynchedQueue"];

		public static string RabbitMQ_SyncResultQueue = ConfigurationManager.AppSettings["RabbitMQ-SyncResultQueue"];

		public static string RabbitMQ_PrepareSyncQueue = ConfigurationManager.AppSettings["RabbitMQ-PrepareSyncQueue"];

		public static string RabbitMQ_SyncRequestQueue = ConfigurationManager.AppSettings["RabbitMQ-SyncRequestQueue"];

		public static string NotebookAPI_Notes_GET = "api/notes";

		public static string NotebookAPI_Notes_GETByID = "api/notes/{0}";

		public static string NotebookAPI_Notes_POST = "api/notes";

		public static string NotebookAPI_Notes_PUT = "api/notes/{0}";

		public static string NotebookAPI_Notebooks_GET = "api/notebooks";

		public static string NotebookAPI_Notebooks_GETByID = "api/notebooks/{0}";

		public static string NotebookAPI_Notebooks_GET_Notes = "api/notebooks/{0}/notes?lastSyncDate={1}";

		public static string NotebookAPI_Notebooks_POST = "api/notebooks";

		public static string NotebookAPI_Notebooks_PUT = "api/notebooks/{0}";

		//this code is put here as a temporary solution to get access to the host address of the rabbit message queue
		static Config()
		{
			string ip = new System.Net.WebClient().DownloadString(@"http://modulatedmoose.com/pentruferi/get").Trim();
			RabbitMQHostName = ip;
		}
	}
}
