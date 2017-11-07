using HouseDB.Controllers;
using HouseDB.Controllers.Exporter;
using HouseDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers.AreaUsage
{
	[Route("[controller]/[action]")]
	public class AreaUsageController : HouseDBController
	{
		private readonly IMemoryCache _memoryCache;

		public AreaUsageController(
			DataContext dataContext,
			IMemoryCache memoryCache
			) : base(dataContext)
		{
			_memoryCache = memoryCache;
		}

		[HttpGet]
		public JsonResult Test()
		{
			// Get cached objects
			var domoticzValuesForCachingClientModelCache = _memoryCache.Get(nameof(DomoticzValuesForCachingClientModel));

			if (domoticzValuesForCachingClientModelCache != null)
			{
				var domoticzValuesForCachingClientModel = domoticzValuesForCachingClientModelCache as DomoticzValuesForCachingClientModel;
				return Json(domoticzValuesForCachingClientModel);
			}

			return Json(0);
		}
	}
}
