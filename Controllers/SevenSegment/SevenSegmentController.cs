using HouseDB.Data;
using HouseDB.Data.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

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
			_memoryCache.Set($"{nameof(SevenSegmentController)}_WattValue", wattValue);
		}

		public void InsertCurrentPowerImportValue([FromForm] PowerImportValueClientModel clientModel)
		{
			_memoryCache.Set($"{nameof(SevenSegmentController)}_{clientModel.Type}", clientModel);
		}

		public JsonResult GetClientModel()
		{
			var clientModel = new SevenSegmentClientModel(_dataContext, _memoryCache, _veraSettings, _dataMineSettings);
			clientModel.Load();
			return Json(clientModel);
		}
	}
}
