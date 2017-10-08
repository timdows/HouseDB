using Newtonsoft.Json;
using System.Collections.Generic;

namespace HouseDB.Data.Models
{
	public class HeaterMeter : SqlBase
    {
		public string Name { get; set; }
		public string GNumber { get; set; }
		public List<HeaterValue> HeaterValues { get; set; }

		[JsonIgnore]
		public virtual HeaterMeterGroup HeaterMeterGroup { get; set; }
	}
}
