using System.Collections.Generic;

namespace HouseDB.Data.Models
{
	public class HeaterMeterGroup : SqlBase
    {
		public string Name { get; set; }
		public List<HeaterMeter> HeaterMeters { get; set; }
	}
}
