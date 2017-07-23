using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseDB.Data.Models
{
	public class KwhDateUsage : SqlBase
	{
		public Device Device { get; set; }
		public DateTime Date { get; set; }
		public Decimal Usage { get; set; }

		[ForeignKey("Device")]
		public long DeviceID { get; set; }
	}
}
