using HouseDB.Data;
using HouseDB.Extensions;
using System;
using System.Linq;

namespace HouseDB.Helpers
{
	public class KwhDayReadingHelper
    {
		public static decimal GetKwhMonthReading(DataContext dataContext, int year, int month)
		{
			var firstDayOfMonth = new DateTime(year, month, 1);

			var dayValues = dataContext.KwhDeviceValues
				.Where(a_item => a_item.DateTime >= firstDayOfMonth &&
								 a_item.DateTime <= firstDayOfMonth.AddMonths(1))
				.ToList();

			var lowest = dayValues.Min(a_item => a_item.Value);
			var highest = dayValues.Max(a_item => a_item.Value);

			return highest - lowest;

		}

		public static decimal GetKwhWeekReading(DataContext dataContext, int year, int week)
		{
			var mondayOfWeek = DateTimeExtension.FirstDateOfWeekISO8601(year, week);

			var dayValues = dataContext.KwhDeviceValues
				.Where(a_item => a_item.DateTime >= mondayOfWeek &&
								 a_item.DateTime <= mondayOfWeek.AddDays(7))
				.ToList();

			var lowest = dayValues.Min(a_item => a_item.Value);
			var highest = dayValues.Max(a_item => a_item.Value);

			return highest - lowest;
		}
	}
}
