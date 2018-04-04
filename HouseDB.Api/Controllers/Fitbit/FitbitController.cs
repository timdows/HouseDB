using HouseDB.Api.Data;
using HouseDB.Api.Data.Models;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers.Fitbit
{
	[Route("[controller]/[action]")]
	public class FitbitController : HouseDBController
	{
		private readonly FitbitSettings _fitbitSettings;

		public FitbitController(DataContext dataContext,
			IOptions<FitbitSettings> fitbitSettings) : base(dataContext)
		{
			_fitbitSettings = fitbitSettings.Value;
		}

		[HttpGet]
		public async Task<IActionResult> GetActivitySteps(string clientId)
		{
			var token = await FitbitHelper.GetAccessToken(_dataContext, _fitbitSettings, clientId);
			var response = await FitbitHelper.GetResponseFromApi(token, "https://api.fitbit.com/1/user/-/activities/steps/date/today/1y.json");
			return Json(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetActivityDistance(string clientId)
		{
			var token = await FitbitHelper.GetAccessToken(_dataContext, _fitbitSettings, clientId);
			var response = await FitbitHelper.GetResponseFromApi(token, "https://api.fitbit.com/1/user/-/activities/distance/date/today/1y.json");
			return Json(response);
		}

		[HttpPost]
		public async Task InsertCallback([FromBody] InsertCallbackClientModel insertCallbackClientModel)
		{
			var fitbitClientDetail = _dataContext.FitbitClientDetails
				.Single(a_item => a_item.ClientId == insertCallbackClientModel.ClientId);

			var fitbitAuthCode = new FitbitAuthCode
			{
				FitbitClientDetail = fitbitClientDetail,
				AuthCode = insertCallbackClientModel.AuthCode,
				DateTimeAdded = DateTime.Now
			};

			_dataContext.FitbitAuthCodes.Add(fitbitAuthCode);
			await _dataContext.SaveChangesAsync();

			await FitbitHelper.ExchangeAuthCodeForAccessToken(_dataContext, _fitbitSettings, fitbitAuthCode);			
		}
		
	}
}
