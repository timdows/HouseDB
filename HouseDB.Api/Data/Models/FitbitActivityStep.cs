using System;

namespace HouseDB.Api.Data.Models
{
	public class FitbitActivityStep : SqlBase
    {
		public int Steps { get; set; }
		public DateTime Date { get; set; }
	}
}
