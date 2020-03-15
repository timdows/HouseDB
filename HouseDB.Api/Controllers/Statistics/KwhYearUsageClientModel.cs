using HouseDB.Api.Data.Models;
using System.Collections.Generic;
using System.Globalization;

namespace HouseDB.Api.Controllers.Statistics
{
	public class KwhYearUsageClientModel
    {
		public Data.Models.Device Device { get; set; }
		public int Year { get; set; }
		public decimal YearUsage { get; set; }
		public List<KwhMonthUsageValue> MonthValues { get; set; } = new List<KwhMonthUsageValue>();
		public List<KwhWeekUsageValue> WeekValues { get; set; } = new List<KwhWeekUsageValue>();
		public List<KwhDateUsage> DayValues { get; set; } = new List<KwhDateUsage>();
	}

	public class KwhMonthUsageValue
	{
		public int Month { get; set; }
		public decimal Usage { get; set; }
		public string MonthString => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
	}

	public class KwhWeekUsageValue
	{
		public int Week { get; set; }
		public decimal Usage { get; set; }
	}
}
