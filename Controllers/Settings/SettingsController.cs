using HouseDB.Data;
using HouseDB.Data.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HouseDB.Controllers.Settings
{
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

		public JsonResult GetVeraSettings()
		{
			return Json(_veraSettings);
		}

		public JsonResult GetDataMineSettings()
		{
			return Json(_dataMineSettings);
		}

		public JsonResult GetRaspicamSettings()
		{
			return Json(_raspicamSettings);
		}

		public JsonResult GetDomoticzSettings()
		{
			return Json(_domoticzSettings);
		}
	}
}
