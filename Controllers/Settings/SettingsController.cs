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

		public SettingsController(
			DataContext dataContext,
			IOptions<VeraSettings> veraSettings,
			IOptions<DataMineSettings> dataMineSettings) : base(dataContext)
		{
			_veraSettings = veraSettings.Value;
			_dataMineSettings = dataMineSettings.Value;
		}

		public JsonResult GetVeraSettings()
		{
			return Json(_veraSettings);
		}

		public JsonResult GetDataMineSettings()
		{
			return Json(_dataMineSettings);
		}
	}
}
