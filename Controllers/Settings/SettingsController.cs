using HouseDB.Data;
using HouseDB.Data.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HouseDB.Controllers.Settings
{
	[Route("[controller]/[action]")]
	public class SettingsController : HouseDBController
	{
		private readonly VeraSettings _veraSettings;
		private readonly DataMineSettings _dataMineSettings;
		private readonly RaspicamSettings _raspicamSettings;
		private readonly DomoticzSettings _domoticzSettings;

		public SettingsController(
			DataContext dataContext,
			IOptions<VeraSettings> veraSettings,
			IOptions<DataMineSettings> dataMineSettings,
			IOptions<RaspicamSettings> raspicamSettings,
			IOptions<DomoticzSettings> domoticzSettings) : base(dataContext)
		{
			_veraSettings = veraSettings.Value;
			_dataMineSettings = dataMineSettings.Value;
			_raspicamSettings = raspicamSettings.Value;
			_domoticzSettings = domoticzSettings.Value;
		}

		[HttpGet]
		[Produces(typeof(VeraSettings))]
		public JsonResult GetVeraSettings()
		{
			return Json(_veraSettings);
		}

		[HttpGet]
		[Produces(typeof(DataMineSettings))]
		public JsonResult GetDataMineSettings()
		{
			return Json(_dataMineSettings);
		}

		[HttpGet]
		[Produces(typeof(RaspicamSettings))]
		public JsonResult GetRaspicamSettings()
		{
			return Json(_raspicamSettings);
		}

		[HttpGet]
		[Produces(typeof(DomoticzSettings))]
		public JsonResult GetDomoticzSettings()
		{
			return Json(_domoticzSettings);
		}
	}
}
