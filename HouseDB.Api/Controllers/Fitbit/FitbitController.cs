using HouseDB.Api.Data;
using HouseDB.Api.Data.FitbitResponse;
using HouseDB.Api.Data.Models;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
		[Produces(typeof(List<FitbitActivityStep>))]
		public async Task<IActionResult> GetActivitySteps(string clientId)
		{
			var existingFitbitActivitySteps = _dataContext.FitbitActivitySteps
				.Where(item => item.FitbitClientDetail.ClientId == clientId)
				.ToList();

			var fitbitClientDetail = _dataContext.FitbitClientDetails
				.Single(item => item.ClientId == clientId);

			var token = await FitbitHelper.GetAccessToken(_dataContext, _fitbitSettings, clientId);

			if (existingFitbitActivitySteps.Any(item => item.Date == DateTime.Today.AddDays(-1)))
			{
				// Only get today's steps
				var response = await FitbitHelper.GetResponseFromApi(token, "https://api.fitbit.com/1/user/-/activities/steps/date/today/1d.json");
				var jToken = response["activities-steps"];
				var activitySteps = FitbitHelper.GetResponseList<ActivityStep>(jToken);
				var fitbitActivitySteps = FitbitActivityStep.GetFitbitActivitySteps(activitySteps, fitbitClientDetail);
				existingFitbitActivitySteps.AddRange(fitbitActivitySteps);
				return Json(existingFitbitActivitySteps.OrderByDescending(item => item.Date));
			}
			else
			{
				var response = await FitbitHelper.GetResponseFromApi(token, "https://api.fitbit.com/1/user/-/activities/steps/date/today/1y.json");

				var jToken = response["activities-steps"];
				var activitySteps = FitbitHelper.GetResponseList<ActivityStep>(jToken);
				var fitbitActivitySteps = FitbitActivityStep.GetFitbitActivitySteps(activitySteps, fitbitClientDetail);

				var existingDates = existingFitbitActivitySteps.Select(item => item.Date).ToList();

				foreach (var fitbitActivityStep in fitbitActivitySteps)
				{
					if (existingDates.Contains(fitbitActivityStep.Date)) continue;
					if (fitbitActivityStep.Steps == 0 || fitbitActivityStep.Date == DateTime.Today) continue;

					_dataContext.FitbitActivitySteps.Add(fitbitActivityStep);
				}

				await _dataContext.SaveChangesAsync();

				return Json(fitbitActivitySteps.OrderByDescending(item => item.Date));
			}
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
