using HouseDB.Api.Controllers.Exporter;
using HouseDB.Api.Data;
using HouseDB.Api.Data.Exporter;
using HouseDB.Core.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Api.Controllers.SevenSegment
{
	public class SevenSegmentClientModel : BaseClientModel
	{
		public int Watt { get; set; } = 0;
		public decimal Today { get; set; } = 0;
		public decimal ThisWeek { get; set; } = 0;
		public decimal ThisMonth { get; set; } = 0;
		public decimal LastMonth { get; set; } = 0;

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
			var domoticzP1ConsumptionsCache = _memoryCache.Get(nameof(DomoticzP1Consumption));

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
				ThisWeek = (decimal)Math.Round(domoticzP1Consumption
					.Where(a_item => a_item.Date >= thisWeekMonday &&
									 a_item.Date <= thisWeekSunday)
					.Sum(a_item => a_item.DayUsage), 2);

				// Calculate Month values
				ThisMonth = (decimal)Math.Round(domoticzP1Consumption
					.Where(a_item => a_item.Date >= thisMonthFirstDay &&
									 a_item.Date <= thisMonthLastDay)
					.Sum(a_item => a_item.DayUsage), 2);

				LastMonth = (decimal)Math.Round(domoticzP1Consumption
					.Where(a_item => a_item.Date >= previousMonthFirstDay &&
									 a_item.Date <= previousMonthLastDay)
					.Sum(a_item => a_item.DayUsage), 2);
			}
		}
	}
}
