using System.Collections.Generic;

namespace Api.Data.Statistics
{
	public class MonthOverview
    {
		public int Month { get; set; }
		public List<DeviceValue> DeviceValues { get; set; } = new List<DeviceValue>();
		public double P1Usage { get; set; }
	}
}
