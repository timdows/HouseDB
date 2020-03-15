using System;

namespace HouseDB.Api.Data.Models
{
	public class TemperatureDeviceReading : SqlBase
	{
		public DateTime DateTime { get; set; }
		public decimal Temperature { get; set; }
		public string Url { get; set; }
		public Device Device { get; set; }
	}
}
