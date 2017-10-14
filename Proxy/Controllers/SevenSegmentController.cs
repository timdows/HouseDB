using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Porxy.Models.Settings;

namespace Proxy.Controllers
{
	public class SevenSegmentController : Controller
    {
		private readonly HouseDBSettings _houseDBSettings;

		public SevenSegmentController(IOptions<HouseDBSettings> houseDBSettings)
		{
			_houseDBSettings = houseDBSettings.Value;
		}

		[HttpGet]
		public JsonResult Index()
		{
			return Json("teaa");
		}
	}
}
