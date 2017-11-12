using System;
using System.Collections.Generic;

namespace Api.Data.AreaUsage
{
	public class DayOverview
	{
		public DateTime Date { get; set; }
		public List<DeviceValue> DeviceValues { get; set; } = new List<DeviceValue>();
	}
}
