using HouseDBCore.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Proxy.Controllers
{
	public class SevenSegmentController : Controller
    {
		private readonly HouseDBSettings _houseDBSettings;

		public SevenSegmentController(HouseDBSettings houseDBSettings)
		{
			_houseDBSettings = houseDBSettings;
		}

		[HttpGet]
		public async Task<string> Index()
		{
			var token = await GetJWTAccessToken(_houseDBSettings);
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await client.GetStringAsync($"{_houseDBSettings.ApiUrl}sevensegment/getclientmodel.json");
				return response;
			}
		}

		private static async Task<string> GetJWTAccessToken(HouseDBSettings houseDBSettings)
		{
			var disco = await DiscoveryClient.GetAsync(houseDBSettings.IS4Url);

			// request token
			var tokenClient = new TokenClient(disco.TokenEndpoint, houseDBSettings.ClientID, houseDBSettings.Password);
			var tokenResponse = await tokenClient.RequestClientCredentialsAsync(houseDBSettings.Scope);

			return tokenResponse.AccessToken;
		}
	}
}
