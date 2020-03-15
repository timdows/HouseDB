using System;

namespace HouseDB.Api.Data.Fitbit
{
	public class FitbitWeekOverviewItem
	{
		public DateTime Date { get; set; }
		public int Steps { get; set; }
		public decimal KiloMeters { get; set; }
		public string DisplayText { get; set; }
	}
}
