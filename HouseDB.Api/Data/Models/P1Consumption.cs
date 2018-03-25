using HouseDB.Api.Data.Models;
using System;

namespace HouseDB.Api.Data.Models
{
	public class P1Consumption : SqlBase
    {
		public DateTime Date { get; set; }
		public double DayUsage { get; set; }
	}
}
