using HouseDBCore;
using HouseDBCore.Settings;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<string> Index()
		{
			var token = await _jwtTokenManager.GetToken(_houseDBSettings);
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await client.GetStringAsync($"{_houseDBSettings.ApiUrl}sevensegment/getclientmodel.json");
				return response;
			}
		}
	}
}
