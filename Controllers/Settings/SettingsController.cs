using HouseDB.Data;
using HouseDB.Data.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HouseDB.Controllers.Settings
{
	public class SettingsController : HouseDBController
	{
		private readonly VeraSettings _veraSettings;

		public SettingsController(
			DataContext dataContext,
			IOptions<VeraSettings> veraSettings) : base(dataContext)
		{
			_veraSettings = veraSettings.Value;
		}

		public IActionResult GetVeraSettings()
		{
			return Json(_veraSettings);
		}
	}
}
