using HouseDB.Core.Interfaces;
using HouseDB.Core.UseCases.DomoticzCache;
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

		public InsertDomoticzDeviceValuesForCachingRequest TryGetDomoticzDeviceValuesForCachingRequest(string key)
		{
			if (_memoryCache.TryGetValue(key, out InsertDomoticzDeviceValuesForCachingRequest cache))
			{
				return cache;
			}

			return null;
		}

		public void SetDomoticzDeviceValuesForCachingRequest(string key, InsertDomoticzDeviceValuesForCachingRequest value)
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
