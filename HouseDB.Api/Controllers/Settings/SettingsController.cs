using HouseDB.Api.Data;
using HouseDB.Api.Data.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HouseDB.Api.Controllers.Settings
{
	[Route("[controller]/[action]")]
	public class SettingsController : HouseDBController
	{
		private readonly RaspicamSettings _raspicamSettings;
		private readonly DomoticzSettings _domoticzSettings;

		public SettingsController(
			DataContext dataContext,
			IOptions<RaspicamSettings> raspicamSettings,
			IOptions<DomoticzSettings> domoticzSettings) : base(dataContext)
		{
			_raspicamSettings = raspicamSettings.Value;
			_domoticzSettings = domoticzSettings.Value;
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
