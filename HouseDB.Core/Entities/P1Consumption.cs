using System;

namespace HouseDB.Core.Entities
{
	public class P1Consumption : SqlBase
	{
		public DateTime Date { get; set; }
		public double DayUsage { get; set; }
	}
}
