using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Controllers.SevenSegment
{
    public class DomoticzPowerImportValueClientModel
    {
		public string CounterToday { get; set; }
		public string Data { get; set; }
		public string LastUpdate { get; set; }
		public string Usage { get; set; }
	}
}
