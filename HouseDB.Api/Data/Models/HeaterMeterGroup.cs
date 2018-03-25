using System.Collections.Generic;

namespace HouseDB.Api.Data.Models
{
	public class HeaterMeterGroup : SqlBase
    {
		public string Name { get; set; }
		public List<HeaterMeter> HeaterMeters { get; set; }
	}
}
