using Newtonsoft.Json;
using System;

namespace HouseDB.Api.Data.Models
{
	public class FitbitActivityDistance : SqlBase
    {
		[JsonIgnore]
		public FitbitClientDetail FitbitClientDetail { get; set; }
		public DateTime DateTime { get; set; }
		public decimal KiloMeters { get; set; }
	}
}
