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
    public class FitbitController : Controller
	{
		private readonly HouseDBSettings _houseDBSettings;
		private readonly JwtTokenManager _jwtTokenManager;

		public FitbitController(
			IOptions<HouseDBSettings> houseDBSettings,
			JwtTokenManager jwtTokenManager)
		{
			_houseDBSettings = houseDBSettings.Value;
			_jwtTokenManager = jwtTokenManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetActivitySteps(string clientId)
		{
			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = await _jwtTokenManager.GetToken(_houseDBSettings);
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await api.FitbitGetActivityStepsGetAsync(clientId);
				return Json(response);
			}
		}
	}
}
