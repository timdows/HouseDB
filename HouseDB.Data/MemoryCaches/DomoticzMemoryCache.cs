using HouseDB.Core.DomoticzModels;
using HouseDB.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace HouseDB.Data.MemoryCaches
{
	public class DomoticzMemoryCache : IDomoticzMemoryCache
	{
		private MemoryCache _memoryCache;

		public DomoticzMemoryCache()
		{
			_memoryCache = new MemoryCache(new MemoryCacheOptions
			{
			});
		}

		public DomoticzDeviceValuesForCaching TryGetDomoticzDeviceValues(string key)
		{
			if (_memoryCache.TryGetValue(key, out DomoticzDeviceValuesForCaching cache))
			{
				return cache;
			}

			return null;
		}

		public void SetDomoticzDeviceValues(string key, DomoticzDeviceValuesForCaching value)
		{
			_memoryCache.Set(key, value);
		}

		public void InvalidateAll()
		{
			_memoryCache.Dispose();
			_memoryCache = new MemoryCache(new MemoryCacheOptions
			{
			});
		}
	}
}
