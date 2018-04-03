using System;

namespace HouseDB.Api.Data.Models
{
	public class FitbitAuthCode : SqlBase
    {
		public string AuthCode { get; set; }
		public DateTime DateTimeAdded { get; set; }
	}
}
