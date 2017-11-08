using HouseDBCore;
using HouseDBCore.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Proxy.Controllers
{
	public class TokenController : Controller
    {
		private readonly HouseDBSettings _houseDBSettings;
		private readonly JwtTokenManager _jwtTokenManager;

		public TokenController(
			HouseDBSettings houseDBSettings,
			JwtTokenManager jwtTokenManager)
		{
			_houseDBSettings = houseDBSettings;
			_jwtTokenManager = jwtTokenManager;
		}

		public async Task<JsonResult> Index()
		{
			var token = await _jwtTokenManager.GetToken(_houseDBSettings);
			return Json(token);
		}
    }
}
