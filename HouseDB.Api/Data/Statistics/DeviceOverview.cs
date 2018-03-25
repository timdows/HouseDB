using HouseDB.Api.Data.Models;
using System.Collections.Generic;

namespace HouseDB.Api.Data.Statistics
{
	public class DeviceOverview
    {
		public Device Device { get; set; }
		public List<DayValue> Values { get; set; } = new List<DayValue>();
	}
}
