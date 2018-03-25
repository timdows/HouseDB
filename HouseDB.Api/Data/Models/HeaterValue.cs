using Newtonsoft.Json;
using System;

namespace HouseDB.Api.Data.Models
{
	public class HeaterValue : SqlBase
    {
		public DateTime Date { get; set; }
		public int Value { get; set; }

		[JsonIgnore]
		public virtual HeaterMeter HeaterMeter { get; set; }
	}
}
