using System;

namespace HouseDB.Api.Data.Models
{
	public class FitbitAuthCode : SqlBase
    {
		public FitbitClientDetail FitbitClientDetail { get; set; }
		public string AuthCode { get; set; }
		public DateTime DateTimeAdded { get; set; }
	}
}
