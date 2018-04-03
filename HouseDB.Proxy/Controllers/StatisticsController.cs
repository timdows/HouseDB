using HouseDB.Core;
using HouseDB.Core.Settings;
using HouseDB.Services.HouseDBApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
			IOptions<HouseDBSettings> houseDBSettings,
			JwtTokenManager jwtTokenManager)
		{
			_houseDBSettings = houseDBSettings.Value;
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

		[HttpGet]
		public async Task<IActionResult> GetP1Overview()
		{
			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = await _jwtTokenManager.GetToken(_houseDBSettings);
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await api.StatisticsGetP1OverviewGetAsync();
				return Json(response);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetLastMotionDetections()
		{
			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = await _jwtTokenManager.GetToken(_houseDBSettings);
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await api.StatisticsGetLastMotionDetectionsGetAsync();
				return Json(response);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetWeekDayOverview()
		{
			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = await _jwtTokenManager.GetToken(_houseDBSettings);
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await api.StatisticsWeekDayOverviewGetAsync();
				return Json(response);
			}
		}
	}
}
