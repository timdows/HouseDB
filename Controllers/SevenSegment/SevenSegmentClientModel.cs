using HouseDB.Controllers.Exporter;
using HouseDB.Data;
using HouseDB.Data.Exporter;
using HouseDB.Data.Models;
using HouseDB.Data.Settings;
using HouseDB.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Controllers.SevenSegment
{
	public class SevenSegmentClientModel : BaseClientModel
	{
		public string Watt { get; set; } = "0";
		public string LastWeekTotal { get; set; } = "0";
		public string ThisWeekTotal { get; set; } = "0";
		public string LastMonthTotal { get; set; } = "0";
		public string ThisMonthTotal { get; set; } = "0";

		private readonly IMemoryCache _memoryCache;
		private readonly DataContext _dataContext;
		private readonly VeraSettings _veraSettings;
		private readonly DataMineSettings _dataMineSettings;

		public SevenSegmentClientModel(
			DataContext dataContext,
			IMemoryCache memoryCache,
			VeraSettings veraSettings,
			DataMineSettings dataMineSettings)
		{
			_dataContext = dataContext;
			_memoryCache = memoryCache;
			_veraSettings = veraSettings;
			_dataMineSettings = dataMineSettings;
		}

		public void Load()
		{
			// Get cached objects
			var exporterCurrentPowerValuesCache = _memoryCache.Get(nameof(ExporterCurrentPowerValues));
			var domoticzP1ConsumptionsCache = _memoryCache.Get(nameof(List<DomoticzP1Consumption>));

			if (exporterCurrentPowerValuesCache != null)
			{
				var exporterCurrentPowerValues = exporterCurrentPowerValuesCache as ExporterCurrentPowerValues;
				Watt = exporterCurrentPowerValues.Watt.ToString();
				ThisWeekTotal = exporterCurrentPowerValues.CounterToday.ToString();
			}

			if (domoticzP1ConsumptionsCache != null)
			{
				var domoticzP1Consumption = domoticzP1ConsumptionsCache as List<DomoticzP1Consumption>;

				// Get working dates
				var thisMonthFirstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
				var thisMonthLastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
				var previousMonthFirstDay = thisMonthFirstDay.AddMonths(-1);
				var previousMonthLastDay = thisMonthFirstDay.AddDays(-1);

				var thisWeekMonday = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
				var thisWeekSunday = thisWeekMonday.AddDays(6);
				var previousWeekMonday = DateTime.Today.AddDays(-7).StartOfWeek(DayOfWeek.Monday);
				var previousWeekSunday = previousWeekMonday.AddDays(6);

				// Calculate week values
				LastWeekTotal = domoticzP1Consumption
					.Where(a_item => a_item.Date >= thisWeekMonday &&
									 a_item.Date <= thisWeekSunday)
					.Sum(a_item => a_item.DayUsage)
					.ToString();

				//LastWeekTotal = domoticzP1Consumption
				//	.Where(a_item => a_item.Date >= previousWeekMonday &&
				//					 a_item.Date <= previousWeekSunday)
				//	.Sum(a_item => a_item.DayUsage)
				//	.ToString();

				// Calculate Month values
				ThisMonthTotal = domoticzP1Consumption
					.Where(a_item => a_item.Date >= thisMonthFirstDay &&
									 a_item.Date <= thisMonthLastDay)
					.Sum(a_item => a_item.DayUsage)
					.ToString();

				LastMonthTotal = domoticzP1Consumption
					.Where(a_item => a_item.Date >= previousMonthFirstDay &&
									 a_item.Date <= previousMonthLastDay)
					.Sum(a_item => a_item.DayUsage)
					.ToString();
			}
		}

		private IQueryable<KwhDeviceValue> GetKwhDeviceValues(List<KwhDeviceValue> dataset, Data.Models.Device device, DateTime firstDate, DateTime lastDate)
		{
			return dataset
				.Where(a_item => a_item.DeviceID == device.ID &&
								 a_item.DateTime >= firstDate &&
								 a_item.DateTime <= lastDate)
				.DefaultIfEmpty(new KwhDeviceValue())
				.AsQueryable();
		}
	}
}
