using HouseDB.Controllers.Exporter;
using HouseDB.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers.Statistics
{
	public class CurrentUsagesClientModel
    {
		public List<CurrentUsage> CurrentUsages { get; set; } = new List<CurrentUsage>();

		public void Load(DataContext dataContext, IMemoryCache memoryCache)
		{
			// Get cached objects
			var domoticzValuesForCachingClientModelCache = memoryCache.Get(nameof(DomoticzValuesForCachingClientModel));
			if (domoticzValuesForCachingClientModelCache == null)
			{
				return;
			}

			DomoticzValuesForCachingClientModel domoticzValuesForCachingClientModel = null;
			domoticzValuesForCachingClientModel = domoticzValuesForCachingClientModelCache as DomoticzValuesForCachingClientModel;

			CurrentUsages.Add(new CurrentUsage
			{
				DeviceName = "P1",
				TodayKwhUsage = domoticzValuesForCachingClientModel.P1Values.TodayKwhUsage,
				CurrentWattValue = domoticzValuesForCachingClientModel.P1Values.CurrentWattValue
			});

			var devices = dataContext.Devices.ToList();

			foreach(var cacheValue in domoticzValuesForCachingClientModel.DomoticzValuesForCachingValues)
			{
				CurrentUsages.Add(new CurrentUsage
				{
					DeviceName = devices.First(a_item => a_item.ID == cacheValue.DeviceID)?.Name ?? "Unknown",
					TodayKwhUsage = cacheValue.TodayKwhUsage,
					CurrentWattValue = cacheValue.CurrentWattValue
				});
			}
		}
	}

	public class CurrentUsage
	{
		public string DeviceName { get; set; }
		public decimal CurrentWattValue { get; set; }
		public decimal TodayKwhUsage { get; set; }
	}
}
