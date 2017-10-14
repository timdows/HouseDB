using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Porxy.Models.Settings;
using System;
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
		public async Task<JsonResult> Index()
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage()
			{
				RequestUri = new Uri($"{_houseDBSettings.ApiUrl}sevensegment/getclientmodel.json"),
				Method = HttpMethod.Get,
			};
			var token = await GetJWTAccessToken(_houseDBSettings);
			//request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
			request.Headers.Add("Authorization", $"Bearer {token}");
			var response = await client.SendAsync(request);
			//var task = client.SendAsync(request)
			//	.ContinueWith((taskwithmsg) =>
			//	{
			//		var response = taskwithmsg.Result;

			//		var jsonTask = response.Content.ReadAsAsync<string>();
			//		jsonTask.Wait();
			//		var jsonObject = jsonTask.Result;
			//	});
			//task.Wait();

			return Json(response);
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
