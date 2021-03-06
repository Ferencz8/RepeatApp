﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeat.SyncronizerService.DTOs
{
	public class DataToBeSynched : SyncRequest
	{

		public List<Notebook> Notebooks { get; set; }

		public new DataToBeSynched Clone()
		{
			return base.Clone<DataToBeSynched>();
		}
	}
}
