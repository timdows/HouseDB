using HouseDBCore;
using HouseDBCore.Settings;
using Microsoft.AspNetCore.Mvc;
using Proxy.HouseDBService;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Proxy.Controllers
{
	public class SevenSegmentController : Controller
    {
		private readonly HouseDBSettings _houseDBSettings;
		private readonly JwtTokenManager _jwtTokenManager;

		public SevenSegmentController(
			HouseDBSettings houseDBSettings,
			JwtTokenManager jwtTokenManager)
		{
			_houseDBSettings = houseDBSettings;
			_jwtTokenManager = jwtTokenManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = await _jwtTokenManager.GetToken(_houseDBSettings);
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await api.SevenSegmentGetClientModelGetAsync();
				return Json(response);
			}
		}
	}
}
