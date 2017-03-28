using HouseDB.Data;
using HouseDB.Data.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HouseDB.Controllers.SevenSegment
{
	public class SevenSegmentController : HouseDBController
    {
		private const string CacheKeyWattValue = "SevenSegment_WattValue";

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
			_memoryCache.Set(CacheKeyWattValue, wattValue);
		}

		public void InsertCurrentPowerImportValue([FromForm] PowerImportValueClientModel clientModel)
		{
			_memoryCache.Set(CacheKeyWattValue, 1);
		}

		public async Task<JsonResult> SevenSegment()
		{
			var clientModel = new SevenSegmentClientModel(_dataContext, _veraSettings, _dataMineSettings);
			await clientModel.Load();
			return Json(clientModel);
		}
	}

	public class PowerImportValueClientModel
	{
		public string Label { get; set; }
		public decimal Min { get; set; }
		public decimal Max { get; set; }
	}
}
