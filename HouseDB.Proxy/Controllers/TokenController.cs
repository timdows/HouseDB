using HouseDB.Core;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HouseDB.Proxy.Controllers
{
	public class TokenController : Controller
    {
		private readonly HouseDBSettings _houseDBSettings;
		private readonly JwtTokenManager _jwtTokenManager;

		public TokenController(
			IOptions<HouseDBSettings> houseDBSettings,
			JwtTokenManager jwtTokenManager)
		{
			_houseDBSettings = houseDBSettings.Value;
			_jwtTokenManager = jwtTokenManager;
		}

		public async Task<JsonResult> Index()
		{
			var token = await _jwtTokenManager.GetToken(_houseDBSettings);
			return Json(token);
		}
    }
}
