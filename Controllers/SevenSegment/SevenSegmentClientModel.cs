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
			//var watt = _memoryCache.Get($"{nameof(ExporterController)}_WattValue");
			var exporterCurrentPowerValuesCache = _memoryCache.Get(nameof(ExporterCurrentPowerValues));
			var domoticzP1ConsumptionsCache = _memoryCache.Get(nameof(List<DomoticzP1Consumption>));
			//var high = _memoryCache.Get($"{nameof(SevenSegmentController)}_High");
			//var low = _memoryCache.Get($"{nameof(SevenSegmentController)}_Low");
			//var highDeviceCache = _memoryCache.Get($"{nameof(SevenSegmentClientModel)}_HighDevice");
			//var lowDeviceCache = _memoryCache.Get($"{nameof(SevenSegmentClientModel)}_LowDevice");

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

			// In current domoticz setting not working
			return;

			// Get the two devices for high and low
			//Data.Models.Device highDevice;
			//Data.Models.Device lowDevice;

			//if (highDeviceCache == null)
			//{
			//	highDevice = _dataContext.Devices.Single(a_item => a_item.ID == _dataMineSettings.PowerImport1Channel);
			//	_memoryCache.Set($"{nameof(SevenSegmentClientModel)}_HighDevice", highDevice);
			//}
			//else
			//{
			//	highDevice = highDeviceCache as Data.Models.Device;
			//}

			//if (lowDeviceCache == null)
			//{
			//	lowDevice = _dataContext.Devices.Single(a_item => a_item.ID == _dataMineSettings.PowerImport2Channel);
			//	_memoryCache.Set($"{nameof(SevenSegmentClientModel)}_LowDevice", lowDevice);
			//}
			//else
			//{
			//	lowDevice = lowDeviceCache as Data.Models.Device;
			//}



			//// Get working data from cache or set it
			//var dataset = _memoryCache.Get($"{nameof(SevenSegmentClientModel)}_DataSet") as List<KwhDeviceValue>;
			//if (dataset == null)
			//{
			//	dataset = _dataContext.KwhDeviceValues
			//		.Where(a_item => (a_item.DeviceID == highDevice.ID || a_item.DeviceID == lowDevice.ID) &&
			//						 a_item.DateTime >= previousMonthFirstDay &&
			//						 a_item.DateTime <= thisWeekSunday)
			//		.ToList();
			//	_memoryCache.Set(
			//		$"{nameof(SevenSegmentClientModel)}_DataSet", dataset, DateTimeOffset.Now.AddHours(1));
			//}

			//// Calculate previous week
			//var previousWeekHighValues = GetKwhDeviceValues(dataset, highDevice, previousWeekMonday, previousWeekSunday);
			//var previousWeekHigh = previousWeekHighValues.Max(a_item => a_item.Value) - previousWeekHighValues.Min(a_item => a_item.Value);

			//var previousWeekLowValues = GetKwhDeviceValues(dataset, lowDevice, previousWeekMonday, previousWeekSunday);
			//var previousWeekLow = previousWeekLowValues.Max(a_item => a_item.Value) - previousWeekLowValues.Min(a_item => a_item.Value);

			//LastWeekTotal = (previousWeekHigh + previousWeekLow).ToString();

			//// Calculate previous month
			//var previousMonthHighValues = GetKwhDeviceValues(dataset, highDevice, previousMonthFirstDay, previousMonthLastDay);
			//var previousMonthHigh = previousMonthHighValues.Max(a_item => a_item.Value) - previousMonthHighValues.Min(a_item => a_item.Value);

			//var previousMonthLowValues = GetKwhDeviceValues(dataset, lowDevice, previousMonthFirstDay, previousMonthLastDay);
			//var previousMonthLow = previousMonthLowValues.Max(a_item => a_item.Value) - previousMonthLowValues.Min(a_item => a_item.Value);

			//LastMonthTotal = (previousMonthHigh + previousMonthLow).ToString();

			//// Check if we got updated values from cache
			//var highPowerImportValue = (high == null) ? null : high as PowerImportValueClientModel;
			//var lowPowerImportValue = (low == null) ? null : low as PowerImportValueClientModel;

			//// Calculate this week and try to use cached values
			//var thisWeekHighValues = GetKwhDeviceValues(dataset, highDevice, thisWeekMonday, thisWeekSunday);
			//var thisWeekHigh = (highPowerImportValue?.ValidMax ?? thisWeekHighValues.Max(a_item => a_item.Value)) -
			//				   (highPowerImportValue?.ValidMin ?? thisWeekHighValues.Min(a_item => a_item.Value));

			//var thisWeekLowValues = GetKwhDeviceValues(dataset, lowDevice, thisWeekMonday, thisWeekSunday);
			//var thisWeekLow = (lowPowerImportValue?.ValidMax ?? thisWeekLowValues.Max(a_item => a_item.Value)) -
			//				  (lowPowerImportValue?.ValidMin ?? thisWeekLowValues.Min(a_item => a_item.Value));

			//ThisWeekTotal = (thisWeekHigh + thisWeekLow).ToString();

			//// Calculate this month and try to use cached values
			//var thisMonthHighValues = GetKwhDeviceValues(dataset, highDevice, thisMonthFirstDay, thisMonthLastDay);
			//var thisMonthHigh = (highPowerImportValue?.ValidMax ?? thisMonthHighValues.Max(a_item => a_item.Value)) -
			//					(highPowerImportValue?.ValidMin ?? thisMonthHighValues.Min(a_item => a_item.Value));

			//var thisMonthLowValues = GetKwhDeviceValues(dataset, lowDevice, thisMonthFirstDay, thisMonthLastDay);
			//var thisMonthLow = (highPowerImportValue?.ValidMax ?? thisMonthLowValues.Max(a_item => a_item.Value)) -
			//				   (highPowerImportValue?.ValidMin ?? thisMonthLowValues.Min(a_item => a_item.Value));

			//ThisMonthTotal = (thisMonthHigh + thisMonthLow).ToString();
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
