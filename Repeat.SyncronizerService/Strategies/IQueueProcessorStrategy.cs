using Repeat.SyncronizerService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.Strategies
{
	public interface IQueueProcessor<T>
		where T : class
	{

		void Process(IQueue queue, T messageToBeProcessed);
	}
}
