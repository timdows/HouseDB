using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseDB.Api.Data.Models
{
	public class SqlBase
	{
		[Key]
		public long ID { get; set; }

		//[Timestamp]
		//public byte[] Timestamp { get; set; }

		[JsonIgnore]
		public DateTime? DateDeleted { get; set; }

		[NotMapped]
		[JsonIgnore]
		public bool IsNew => ID == 0;

		[NotMapped]
		[JsonIgnore]
		public bool IsDeleted => DateDeleted != null;
	}
}
