using System;
using System.Globalization;

namespace HouseDB.Core.Extensions
{
	public static class DateTimeExtension
	{
		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			var diff = dt.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}

			return dt.AddDays(-1 * diff).Date;
		}

		public static DateTime FromUnixTime(this long unixTime, bool removeThousands = false)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
			var seconds = removeThousands ? unixTime / 1000 : unixTime;
			DateTime retVal;
			try
			{
				retVal = epoch.AddSeconds(seconds);
			}
			catch(Exception excep)
			{
				var a = excep;
				retVal = new DateTime(1970, 1, 1, 0, 0, 0);
			}
			
			return retVal;
		}

		public static long ToUnixTime(this DateTime date)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
			return Convert.ToInt64((date - epoch).TotalSeconds);
		}

		// This presumes that weeks start with Monday.
		// Week 1 is the 1st week of the year with a Thursday in it.
		public static int GetIso8601WeekOfYear(this DateTime time)
		{
			// Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
			// be the same week# as whatever Thursday, Friday or Saturday are,
			// and we always get those right
			var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
			if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
			{
				time = time.AddDays(3);
			}

			// Return the week of our adjusted day
			return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}

		public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
		{
			var jan1 = new DateTime(year, 1, 1);
			var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

			var firstThursday = jan1.AddDays(daysOffset);
			var cal = CultureInfo.CurrentCulture.Calendar;
			var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			var weekNum = weekOfYear;
			if (firstWeek <= 1)
			{
				weekNum -= 1;
			}
			var result = firstThursday.AddDays(weekNum * 7);
			return result.AddDays(-3);
		}

		public static int GetWeeksInYear(int year)
		{
			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			DateTime date1 = new DateTime(year, 12, 31);
			Calendar cal = dfi.Calendar;
			return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
		}
	}
}
