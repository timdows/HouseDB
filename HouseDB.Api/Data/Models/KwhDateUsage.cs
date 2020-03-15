using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseDB.Api.Data.Models
{
	public class KwhDateUsage : SqlBase
	{
		[JsonIgnore]
		public Device Device { get; set; }
		public DateTime Date { get; set; }
		public decimal Usage { get; set; }

		[ForeignKey("Device")]
		public long DeviceID { get; set; }
	}
}
