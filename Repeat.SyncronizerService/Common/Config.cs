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
																						  
		public static string RabbitMQHostName = ConfigurationManager.AppSettings["RabbitMQ-HostName"];

		public static string RabbitMQPort = ConfigurationManager.AppSettings["RabbitMQ-Port"];

		public static string RabbitMQVirtualHost = ConfigurationManager.AppSettings["RabbitMQ-VirtualHost"];

		public static string RabbitMQ_DataQueue = ConfigurationManager.AppSettings["RabbitMQ-DataQueue"];

		public static string RabbitMQ_SyncResultQueue = ConfigurationManager.AppSettings["RabbitMQ-SyncResultQueue"];

		public static string RabbitMQ_PrepareSyncQueue = ConfigurationManager.AppSettings["RabbitMQ-PrepareSyncQueue"];

		public static string RabbitMQ_Last_Sync_Date = ConfigurationManager.AppSettings["RabbitMQ-LastSyncronizedDateQueue"];
	}
}
