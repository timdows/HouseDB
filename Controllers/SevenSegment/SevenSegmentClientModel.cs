using HouseDB.Data;
using HouseDB.Data.Models;
using HouseDB.Data.Settings;
using HouseDB.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace HouseDB.Controllers.SevenSegment
{
	public class SevenSegmentClientModel
	{
		public string Watt { get; set; }
		public string LastWeekTotal { get; set; }
		public string ThisWeekTotal { get; set; }
		public string LastMonthTotal { get; set; }
		public string ThisMonthTotal { get; set; }

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
			var watt = _memoryCache.Get($"{nameof(SevenSegmentController)}_WattValue");
			var high = _memoryCache.Get($"{nameof(SevenSegmentController)}_High");
			var low = _memoryCache.Get($"{nameof(SevenSegmentController)}_Low");

			Watt = (watt == null) ? "0" : watt.ToString();

			// Get the two devices for high and low
			var highDevice = _dataContext.Devices.Single(a_item => a_item.ID == _dataMineSettings.PowerImport1Channel);
			var lowDevice = _dataContext.Devices.Single(a_item => a_item.ID == _dataMineSettings.PowerImport2Channel);

			// Get working dates
			var thisMonthFirstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			var thisMonthLastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
			var previousMonthFirstDay = thisMonthFirstDay.AddMonths(-1);
			var previousMonthLastDay = thisMonthFirstDay.AddDays(-1);

			var thisWeekMonday = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
			var thisWeekSunday = thisWeekMonday.AddDays(6);
			var previousWeekMonday = DateTime.Today.AddDays(-7).StartOfWeek(DayOfWeek.Monday);
			var previousWeekSunday = previousWeekMonday.AddDays(6);

			// Calculate previous week
			var previousWeekHighValues = GetKwhDeviceValues(highDevice, previousWeekMonday, previousWeekSunday);
			var previousWeekHigh = previousWeekHighValues.Max(a_item => a_item.Value) - previousWeekHighValues.Min(a_item => a_item.Value);

			var previousWeekLowValues = GetKwhDeviceValues(lowDevice, previousWeekMonday, previousWeekSunday);
			var previousWeekLow = previousWeekLowValues.Max(a_item => a_item.Value) - previousWeekLowValues.Min(a_item => a_item.Value);

			LastWeekTotal = (previousWeekHigh + previousWeekLow).ToString();

			// Calculate previous month
			var previousMonthHighValues = GetKwhDeviceValues(highDevice, previousMonthFirstDay, previousMonthLastDay);
			var previousMonthHigh = previousMonthHighValues.Max(a_item => a_item.Value) - previousMonthHighValues.Min(a_item => a_item.Value);

			var previousMonthLowValues = GetKwhDeviceValues(lowDevice, previousMonthFirstDay, previousMonthLastDay);
			var previousMonthLow = previousMonthLowValues.Max(a_item => a_item.Value) - previousMonthLowValues.Min(a_item => a_item.Value);

			LastMonthTotal = (previousMonthHigh + previousMonthLow).ToString();

			// Check if we got updated values from cache
			var highPowerImportValue = (high == null) ? null : high as PowerImportValueClientModel;
			var lowPowerImportValue = (low == null) ? null : low as PowerImportValueClientModel;

			// Calculate this week and try to use cached values
			var thisWeekHighValues = GetKwhDeviceValues(highDevice, thisWeekMonday, thisWeekSunday);
			var thisWeekHigh = (highPowerImportValue?.Max ?? thisWeekHighValues.Max(a_item => a_item.Value)) - thisWeekHighValues.Min(a_item => a_item.Value);

			var thisWeekLowValues = GetKwhDeviceValues(lowDevice, thisWeekMonday, thisWeekSunday);
			var thisWeekLow = (lowPowerImportValue?.Max ?? thisWeekLowValues.Max(a_item => a_item.Value)) - thisWeekLowValues.Min(a_item => a_item.Value);

			ThisWeekTotal = (thisWeekHigh + thisWeekLow).ToString();

			// Calculate this month and try to use cached values
			var thisMonthHighValues = GetKwhDeviceValues(highDevice, thisMonthFirstDay, thisMonthLastDay);
			var thisMonthHigh = (highPowerImportValue?.Max ?? thisMonthHighValues.Max(a_item => a_item.Value)) - thisMonthHighValues.Min(a_item => a_item.Value);

			var thisMonthLowValues = GetKwhDeviceValues(lowDevice, thisMonthFirstDay, thisMonthLastDay);
			var thisMonthLow = (highPowerImportValue?.Max ?? thisMonthLowValues.Max(a_item => a_item.Value)) - thisMonthLowValues.Min(a_item => a_item.Value);

			ThisMonthTotal = (thisMonthHigh + thisMonthLow).ToString();
		}

		private IQueryable<KwhDeviceValue> GetKwhDeviceValues(Data.Models.Device device, DateTime firstDate, DateTime lastDate)
		{
			return _dataContext.KwhDeviceValues
				.Where(a_item => a_item.ID == device.ID &&
								 a_item.DateTime >= firstDate &&
								 a_item.DateTime <= lastDate);
		}
	}
}
