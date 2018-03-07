using Api.Data.AreaUsage;
using HouseDB.Controllers;
using HouseDB.Controllers.Exporter;
using HouseDB.Data;
using HouseDB.Data.Exporter;
using HouseDB.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers.AreaUsage
{
	[Route("[controller]/[action]")]
	public class AreaUsageController : HouseDBController
	{
		private readonly IMemoryCache _memoryCache;
		private readonly List<Device> _devices;

		public AreaUsageController(
			DataContext dataContext,
			IMemoryCache memoryCache
			) : base(dataContext)
		{
			_memoryCache = memoryCache;
			_devices = _dataContext.Devices
				.Where(a_item => a_item.IsForKwhImport &&
								 (a_item.DomoticzWattIdx != 0 || a_item.DomoticzKwhIdx != 0))
				.OrderBy(a_item => a_item.Name)
				.ToList();
		}

		[HttpGet]
		[Produces(typeof(List<DeviceOverview>))]
		public JsonResult WeekDeviceOverview()
		{
			// Get cached objects
			var domoticzValuesForCachingClientModelCache = _memoryCache.Get(nameof(DomoticzValuesForCachingClientModel));

			DomoticzValuesForCachingClientModel domoticzValuesForCachingClientModel = null;
			if (domoticzValuesForCachingClientModelCache != null)
			{
				domoticzValuesForCachingClientModel = domoticzValuesForCachingClientModelCache as DomoticzValuesForCachingClientModel;
			}

			var oneWeekAgo = DateTime.Today.AddDays(-7);
			var deviceOverviews = new List<DeviceOverview>();

			foreach (var device in _devices)
			{
				var deviceOverview = new DeviceOverview
				{
					Device = device
				};

				// Get last weeks values from database
				var dbValues = _dataContext.KwhDateUsages
					.Where(a_item => a_item.DeviceID == device.ID &&
									 a_item.Date > oneWeekAgo)
					.OrderBy(a_item => a_item.Date)
					.ToList();

				foreach (var value in dbValues)
				{
					// Add the database day values
					deviceOverview.Values.Add(new DayValue
					{
						Date = value.Date,
						Usage = value.Usage
					});

					// Add the cached day value
					var cachedValue = domoticzValuesForCachingClientModel?.DomoticzValuesForCachingValues
						.SingleOrDefault(a_item => a_item.DeviceID == device.ID);
						
					if (cachedValue != null)
					{
						deviceOverview.Values.Add(new DayValue
						{
							Date = DateTime.Today,
							Usage = cachedValue.TodayKwhUsage
						});
					}
				}

				deviceOverviews.Add(deviceOverview);
			}

			return Json(deviceOverviews);
		}

		[HttpGet]
		[Produces(typeof(List<DayOverview>))]
		public JsonResult WeekDayOverview()
		{
			// Get cached objects
			var domoticzValuesForCachingClientModelCache = _memoryCache.Get(nameof(DomoticzValuesForCachingClientModel));

			DomoticzValuesForCachingClientModel domoticzValuesForCachingClientModel = null;
			if (domoticzValuesForCachingClientModelCache != null)
			{
				domoticzValuesForCachingClientModel = domoticzValuesForCachingClientModelCache as DomoticzValuesForCachingClientModel;
			}

			var oneWeekAgo = DateTime.Today.AddDays(-7);
			var dayOverviews = new List<DayOverview>();

			var deviceIDs = _devices.Select(a_item => a_item.ID).ToList();

			// Get last weeks values from database to use later
			var dbValues = _dataContext.KwhDateUsages
				.Where(a_item => deviceIDs.Contains(a_item.DeviceID) &&
								 a_item.Date > oneWeekAgo)
				.OrderBy(a_item => a_item.Date)
				.ToList();

			var domoticzP1ConsumptionsCache = _memoryCache.Get(nameof(List<DomoticzP1Consumption>));
			List<DomoticzP1Consumption> domoticzP1Consumption = null;
			if (domoticzP1ConsumptionsCache != null)
			{
				domoticzP1Consumption = domoticzP1ConsumptionsCache as List<DomoticzP1Consumption>;
			}

			for (var date = DateTime.Today.AddDays(-6); date <= DateTime.Today; date = date.AddDays(1))
			{
				var p1Usage = 0.0;
				if (domoticzP1Consumption != null)
				{
					p1Usage = domoticzP1Consumption.SingleOrDefault(a_item => a_item.Date == date)?.DayUsage ?? 0;
				}

				var dayOverview = new DayOverview
				{
					Date = date,
					P1Usage = p1Usage
				};

				foreach (var device in _devices)
				{
					if (date == DateTime.Today)
					{
						// Add the cached day value
						var cachedValue = domoticzValuesForCachingClientModel?.DomoticzValuesForCachingValues
							.SingleOrDefault(a_item => a_item.DeviceID == device.ID);

						if (cachedValue != null)
						{
							dayOverview.DeviceValues.Add(new DeviceValue
							{
								DeviceName = device.Name,
								Usage = cachedValue.TodayKwhUsage
							});
						}

						continue;
					}

					var value = dbValues
						.SingleOrDefault(a_item => a_item.DeviceID == device.ID &&
												   a_item.Date == date);

					if (value != null)
					{
						dayOverview.DeviceValues.Add(new DeviceValue
						{
							DeviceName = device.Name,
							Usage = value.Usage
						});
					}
				}

				dayOverviews.Add(dayOverview);
			}

			return Json(dayOverviews);
		}

		[HttpGet]
		[Produces(typeof(List<MonthOverview>))]
		public JsonResult YearMonthOverview(int year)
		{
			// Get cached objects
			var domoticzValuesForCachingClientModelCache = _memoryCache.Get(nameof(DomoticzValuesForCachingClientModel));

			DomoticzValuesForCachingClientModel domoticzValuesForCachingClientModel = null;
			if (domoticzValuesForCachingClientModelCache != null)
			{
				domoticzValuesForCachingClientModel = domoticzValuesForCachingClientModelCache as DomoticzValuesForCachingClientModel;
			}

			var beginningOfYear = new DateTime(year, 1, 1);
			var endOfYear = new DateTime(year, 12, 31);
			var monthOverviews = new List<MonthOverview>();

			var deviceIDs = _devices.Select(a_item => a_item.ID).ToList();

			// Get last weeks values from database to use later
			var dbValues = _dataContext.KwhDateUsages
				.Where(a_item => deviceIDs.Contains(a_item.DeviceID) &&
								 a_item.Date >= beginningOfYear &&
								 a_item.Date <= endOfYear)
				.OrderBy(a_item => a_item.Date)
				.ToList();

			var domoticzP1ConsumptionsCache = _memoryCache.Get(nameof(List<DomoticzP1Consumption>));
			List<DomoticzP1Consumption> domoticzP1Consumption = null;
			if (domoticzP1ConsumptionsCache != null)
			{
				domoticzP1Consumption = domoticzP1ConsumptionsCache as List<DomoticzP1Consumption>;
			}

			for (var month = 1; month <= 12; month++)
			{
				var p1Usage = 0.0;
				if (domoticzP1Consumption != null)
				{
					p1Usage = domoticzP1Consumption
						.Where(a_item => a_item.Date.Month == month)?
						.Sum(a_item => a_item.DayUsage) ?? 0;
				}

				var monthOverview = new MonthOverview
				{
					Month = month,
					P1Usage = p1Usage
				};

				foreach (var device in _devices)
				{
					decimal usage = 0;
					// Get the usage from cache if the month is now
					if (month == DateTime.Today.Month)
					{
						// Add the cached day value
						var cachedValue = domoticzValuesForCachingClientModel?.DomoticzValuesForCachingValues
							.SingleOrDefault(a_item => a_item.DeviceID == device.ID);

						if (cachedValue != null)
						{
							usage = cachedValue.TodayKwhUsage;
						}
					}

					usage += dbValues
						.Where(a_item => a_item.DeviceID == device.ID &&
										 a_item.Date.Month == month)?
						.Sum(a_item => a_item.Usage) ?? 0;

					monthOverview.DeviceValues.Add(new DeviceValue
					{
						DeviceName = device.Name,
						Usage = usage
					});
				}

				monthOverviews.Add(monthOverview);
			}

			return Json(monthOverviews);
		}
	}
}
