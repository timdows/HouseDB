using System;
using System.Collections.Generic;

namespace HouseDB.Api.Data.Statistics
{
	public class DayOverview
	{
		public DateTime Date { get; set; }
		public List<DeviceValue> DeviceValues { get; set; } = new List<DeviceValue>();
		public double P1Usage { get; set; }
	}
}
