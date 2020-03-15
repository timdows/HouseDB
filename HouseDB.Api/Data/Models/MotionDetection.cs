using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseDB.Api.Data.Models
{
	public class MotionDetection : SqlBase
    {
		[JsonIgnore]
		public Device Device { get; set; }
		public DateTime DateTimeDetection { get; set; }
		public bool Status { get; set; }

		[ForeignKey("Device")]
		public long DeviceID { get; set; }
	}
}
