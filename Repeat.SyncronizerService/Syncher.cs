using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repeat.SyncronizerService.Common;
using Repeat.SyncronizerService.DAL.Entities;
using Repeat.SyncronizerService.Interfaces;
using Repeat.SyncronizerService.DAL;
using Repeat.SyncronizerService.DTOs;
using Repeat.SyncronizerService.APICallers;
using System.Configuration;
using Repeat.SyncronizerService.Strategies;

namespace Repeat.SyncronizerService
{
	public class Syncher
	{
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

			new Syncher().Launch();
		}

		private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine(e.ExceptionObject.ToString());

			new Syncher().Launch();//restarting syncher
		}

		private IQueue _queue;

		public void Launch()
		{

			//TODO:: check if a lock is needed on _queue object..or use a syncronized obj
			using (_queue = new RabbitMQ())
			{
				RegisterQueueProcessors();

				//task used to check every 10 seconds if the connection is still open, 
				//if not then dispose the _queue object and register the queu processors again
				new Task(() =>
				{
					double sleepTime = 5;
					while (true)
					{

						if (!_queue.Connection.IsOpen)
						{
							Log.Info("Restarting Queue Processors!");

							_queue.Dispose();

							_queue = new RabbitMQ();
							
							RegisterQueueProcessors();
						}
						else
						{
							Log.Info("Socket Status: " + _queue.Connection.IsOpen);

							sleepTime = sleepTime == 80 ? 5 : sleepTime * 2;
						}

						Task.Delay(TimeSpan.FromSeconds(sleepTime)).Wait();
					}
				}).Start();

				Console.WriteLine("Press any key to exit!");
				Console.ReadKey();
			}
		}

		private void RegisterQueueProcessors()
		{
			_queue.ProcessMessage<SyncRequest>(Config.RabbitMQ_PrepareSyncQueue, new PrepareSyncQueueProcessorStrategy());//.Process);

			_queue.ProcessMessage<DataToBeSynched>(Config.RabbitMQ_DataToBeSynchedQueue, new DataToBeSynchedQueueProcessorStrategy());//.Process);
		}
	}
}
