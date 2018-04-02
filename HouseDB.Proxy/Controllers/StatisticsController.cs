using HouseDB.Core;
using HouseDB.Core.Settings;
using HouseDB.Services.HouseDBApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Proxy.Controllers
{
	public class StatisticsController : Controller
	{
		private readonly HouseDBSettings _houseDBSettings;
		private readonly JwtTokenManager _jwtTokenManager;

		public StatisticsController(
			HouseDBSettings houseDBSettings,
			JwtTokenManager jwtTokenManager)
		{
			_houseDBSettings = houseDBSettings;
			_jwtTokenManager = jwtTokenManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetCurrentUsages()
		{
			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = await _jwtTokenManager.GetToken(_houseDBSettings);
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await api.StatisticsGetCurrentUsagesGetAsync();
				return Json(response);
			}
		}
	}
}
