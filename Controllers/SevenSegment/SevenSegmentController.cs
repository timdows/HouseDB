using HouseDB.Data;
using HouseDB.Data.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace HouseDB.Controllers.SevenSegment
{
	public class SevenSegmentController : HouseDBController
	{
		private readonly VeraSettings _veraSettings;
		private readonly IMemoryCache _memoryCache;
		private readonly DataMineSettings _dataMineSettings;

		public SevenSegmentController(
			DataContext dataContext,
			IMemoryCache memoryCache,
			IOptions<VeraSettings> veraSetting,
			IOptions<DataMineSettings> dataMineSettings
			) : base(dataContext)
		{
			_veraSettings = veraSetting.Value;
			_memoryCache = memoryCache;
			_dataMineSettings = dataMineSettings.Value;
		}

		public void InsertCurrentWattValue([FromForm] int wattValue)
		{
			var clientModel = new WattValueClientModel
			{
				Watt = wattValue,
				DateTimeAdded = DateTime.Now
			};
			_memoryCache.Set($"{nameof(SevenSegmentController)}_WattValue", clientModel);
		}

		public void InsertCurrentPowerImportValue([FromForm] PowerImportValueClientModel clientModel)
		{
			clientModel.DateTimeAdded = DateTime.Now;
			_memoryCache.Set($"{nameof(SevenSegmentController)}_{clientModel.Type}", clientModel);
		}

		public JsonResult GetClientModel()
		{
			var clientModel = new SevenSegmentClientModel(_dataContext, _memoryCache, _veraSettings, _dataMineSettings);
			clientModel.Load();
			return Json(clientModel);
		}

		public JsonResult GetDebugCacheData()
		{
			var watt = _memoryCache.Get($"{nameof(SevenSegmentController)}_WattValue");
			var high = _memoryCache.Get($"{nameof(SevenSegmentController)}_High");
			var low = _memoryCache.Get($"{nameof(SevenSegmentController)}_Low");

			return Json(new { watt, high, low });
		}
	}
}
