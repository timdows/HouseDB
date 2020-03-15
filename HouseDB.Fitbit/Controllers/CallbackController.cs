using HouseDB.Core;
using HouseDB.Core.Settings;
using HouseDB.Services.HouseDBApi;
using HouseDB.Services.HouseDBApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Fitbit.Controllers
{
	public class CallbackController : Controller
    {
		private readonly HouseDBSettings _houseDBSettings;
		private readonly JwtTokenManager _jwtTokenManager;

		public CallbackController(
			IOptions<HouseDBSettings> houseDBSettings,
			JwtTokenManager jwtTokenManager)
		{
			_houseDBSettings = houseDBSettings.Value;
			_jwtTokenManager = jwtTokenManager;
		}

		public async Task Index()
		{
			// Get items from querystring
			var authCode = Request.Query.Single(a_item => a_item.Key == "code").Value;
			var clientId = Request.Query.Single(a_item => a_item.Key == "state").Value;

			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = await _jwtTokenManager.GetToken(_houseDBSettings);
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				await api.FitbitInsertCallbackPostAsync(new InsertCallbackClientModel
				{
					AuthCode = authCode,
					ClientId = clientId
				});
			}
		}
	}
}
