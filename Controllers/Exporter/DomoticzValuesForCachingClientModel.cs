using System;
using System.Collections.Generic;

namespace HouseDB.Controllers.Exporter
{
	public class DomoticzValuesForCachingClientModel
	{
		public DateTime DateTime { get; set; }
		public List<DomoticzValuesForCachingValue> DomoticzValuesForCachingValues { get; set; }
	}

	public class DomoticzValuesForCachingValue
	{
		public long DeviceID { get; set; }
		public decimal CurrentWattValue { get; set; }
		public decimal TodayKwhUsage { get; set; }
	}
}
