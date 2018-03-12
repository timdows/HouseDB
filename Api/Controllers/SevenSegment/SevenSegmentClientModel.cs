using HouseDB.Controllers.Exporter;
using HouseDB.Data;
using HouseDB.Data.Exporter;
using HouseDB.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Controllers.SevenSegment
{
	public class SevenSegmentClientModel : BaseClientModel
	{
		public int Watt { get; set; } = 0;
		public decimal Today { get; set; } = 0;
		public double ThisWeek { get; set; } = 0;
		public double ThisMonth { get; set; } = 0;
		public double LastMonth { get; set; } = 0;

		private readonly IMemoryCache _memoryCache;
		private readonly DataContext _dataContext;

		public SevenSegmentClientModel(
			DataContext dataContext,
			IMemoryCache memoryCache)
		{
			_dataContext = dataContext;
			_memoryCache = memoryCache;
		}

		public void Load()
		{
			// Get cached objects
			var domoticzValuesForCachingClientModelCache = _memoryCache.Get(nameof(DomoticzValuesForCachingClientModel));
			var domoticzP1ConsumptionsCache = _memoryCache.Get(nameof(List<DomoticzP1Consumption>));

			if (domoticzValuesForCachingClientModelCache != null)
			{
				var domoticzValuesForCachingClientModel = domoticzValuesForCachingClientModelCache as DomoticzValuesForCachingClientModel;
				Watt = Convert.ToInt32(domoticzValuesForCachingClientModel.P1Values.CurrentWattValue);
				Today = Math.Round(domoticzValuesForCachingClientModel.P1Values.TodayKwhUsage, 2);
			}

			if (domoticzP1ConsumptionsCache != null)
			{
				var domoticzP1Consumption = domoticzP1ConsumptionsCache as List<DomoticzP1Consumption>;

				// Get working dates
				var thisWeekMonday = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
				var thisWeekSunday = thisWeekMonday.AddDays(6);
				var thisMonthFirstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
				var thisMonthLastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
				var previousMonthFirstDay = thisMonthFirstDay.AddMonths(-1);
				var previousMonthLastDay = thisMonthFirstDay.AddDays(-1);

				// Calculate week values
				ThisWeek = Math.Round(domoticzP1Consumption
					.Where(a_item => a_item.Date >= thisWeekMonday &&
									 a_item.Date <= thisWeekSunday)
					.Sum(a_item => a_item.DayUsage), 2);

				// Calculate Month values
				ThisMonth = Math.Round(domoticzP1Consumption
					.Where(a_item => a_item.Date >= thisMonthFirstDay &&
									 a_item.Date <= thisMonthLastDay)
					.Sum(a_item => a_item.DayUsage), 2);

				LastMonth = Math.Round(domoticzP1Consumption
					.Where(a_item => a_item.Date >= previousMonthFirstDay &&
									 a_item.Date <= previousMonthLastDay)
					.Sum(a_item => a_item.DayUsage), 2);
			}
		}
	}
}
