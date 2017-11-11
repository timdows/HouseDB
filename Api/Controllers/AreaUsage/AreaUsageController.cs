using HouseDB.Controllers;
using HouseDB.Controllers.Exporter;
using HouseDB.Data;
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
				.ToList();
		}

		[HttpGet]
		public JsonResult WeekOverview()
		{
			// Get cached objects
			var domoticzValuesForCachingClientModelCache = _memoryCache.Get(nameof(DomoticzValuesForCachingClientModel));

			DomoticzValuesForCachingClientModel domoticzValuesForCachingClientModel = null;
			if (domoticzValuesForCachingClientModelCache != null)
			{
				domoticzValuesForCachingClientModel = domoticzValuesForCachingClientModelCache as DomoticzValuesForCachingClientModel;
			}

			var oneWeekAgo = DateTime.Today.AddDays(-7);
			var weekDeviceOverviews = new List<WeekDeviceOverview>();

			foreach (var device in _devices)
			{
				var weekDeviceOverview = new WeekDeviceOverview
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
					weekDeviceOverview.Values.Add(new DayValue
					{
						Date = value.Date,
						Value = value.Usage
					});

					// Add the cached day value
					var cachedValue = domoticzValuesForCachingClientModel?.DomoticzValuesForCachingValues
						.SingleOrDefault(a_item => a_item.DeviceID == device.ID);
						
					if (cachedValue != null)
					{
						weekDeviceOverview.Values.Add(new DayValue
						{
							Date = DateTime.Today,
							Value = cachedValue.TodayKwhUsage
						});
					}
				}

				weekDeviceOverviews.Add(weekDeviceOverview);
			}

			return Json(weekDeviceOverviews);
		}
	}

	public class WeekDeviceOverview
	{
		public Device Device { get; set; }
		public List<DayValue> Values { get; set; } = new List<DayValue>();
	}

	public class DayValue
	{
		public DateTime Date { get; set; }
		public decimal Value { get; set; }
	}
}
