using HouseDB.Api.Data;
using HouseDB.Api.Data.FitbitResponse;
using HouseDB.Api.Data.Models;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
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

			if (existingFitbitActivitySteps.Any(item => item.Date == DateTime.Today.AddDays(-1)))
			{
				// Only get today's steps
				var activitySteps = await FitbitHelper.GetResponseList<ActivityStep>(
					_dataContext, 
					_fitbitSettings, 
					clientId, 
					"https://api.fitbit.com/1/user/-/activities/steps/date/today/1d.json", 
					"activities-steps");

				var fitbitActivitySteps = FitbitActivityStep.GetFitbitActivitySteps(activitySteps, fitbitClientDetail);
				existingFitbitActivitySteps.AddRange(fitbitActivitySteps);

				return Json(existingFitbitActivitySteps.OrderByDescending(item => item.Date));
			}
			else
			{
				var activitySteps = await FitbitHelper.GetResponseList<ActivityStep>(
					_dataContext,
					_fitbitSettings,
					clientId,
					"https://api.fitbit.com/1/user/-/activities/steps/date/today/1y.json",
					"activities-steps");

				var fitbitActivitySteps = FitbitActivityStep.GetFitbitActivitySteps(activitySteps, fitbitClientDetail);

				foreach (var fitbitActivityStep in fitbitActivitySteps)
				{
					// Check if activity is already in the database
					var sameDate = existingFitbitActivitySteps.SingleOrDefault(item => item.Date == fitbitActivityStep.Date);
					if (sameDate != null)
					{
						if (sameDate.Steps == fitbitActivityStep.Steps)
						{
							continue;
						}
						else
						{
							// Update the steps that are already in the database
							sameDate.Steps = fitbitActivityStep.Steps;
						}
					}
					
					if (fitbitActivityStep.Steps == 0 || fitbitActivityStep.Date == DateTime.Today) continue;

					_dataContext.FitbitActivitySteps.Add(fitbitActivityStep);
				}

				await _dataContext.SaveChangesAsync();

				return Json(fitbitActivitySteps.OrderByDescending(item => item.Date));
			}
		}

		[HttpGet]
		[Produces(typeof(List<FitbitActivityDistance>))]
		public async Task<IActionResult> GetActivityDistance(string clientId)
		{
			var existingFitbitActivityDistances = _dataContext.FitbitActivityDistances
				.Where(item => item.FitbitClientDetail.ClientId == clientId)
				.ToList();

			var fitbitClientDetail = _dataContext.FitbitClientDetails
				.Single(item => item.ClientId == clientId);

			if (existingFitbitActivityDistances.Any(item => item.Date == DateTime.Today.AddDays(-1)))
			{
				// Only get today's distance
				var activityDistance = await FitbitHelper.GetResponseList<ActivityDistance>(
					_dataContext,
					_fitbitSettings,
					clientId,
					"https://api.fitbit.com/1/user/-/activities/distance/date/today/1d.json",
					"activities-distance");

				var fitbitActivityDistances = FitbitActivityDistance.GetFitbitActivityDistances(activityDistance, fitbitClientDetail);
				existingFitbitActivityDistances.AddRange(fitbitActivityDistances);

				return Json(existingFitbitActivityDistances.OrderByDescending(item => item.Date));
			}
			else
			{
				var activityDistance = await FitbitHelper.GetResponseList<ActivityDistance>(
					_dataContext,
					_fitbitSettings,
					clientId,
					"https://api.fitbit.com/1/user/-/activities/distance/date/today/1y.json",
					"activities-distance");

				var fitbitActivityDistances = FitbitActivityDistance.GetFitbitActivityDistances(activityDistance, fitbitClientDetail);

				foreach (var fitbitActivityDistance in fitbitActivityDistances)
				{
					// Check if activity is already in the database
					var sameDate = existingFitbitActivityDistances.SingleOrDefault(item => item.Date == fitbitActivityDistance.Date);
					if (sameDate != null)
					{
						if (sameDate.KiloMeters == fitbitActivityDistance.KiloMeters)
						{
							continue;
						}
						else
						{
							// Update the distance that is already in the database
							sameDate.KiloMeters = fitbitActivityDistance.KiloMeters;
						}
					}
					if (fitbitActivityDistance.KiloMeters == 0 || fitbitActivityDistance.Date == DateTime.Today) continue;

					_dataContext.FitbitActivityDistances.Add(fitbitActivityDistance);
				}

				await _dataContext.SaveChangesAsync();

				return Json(fitbitActivityDistances.OrderByDescending(item => item.Date));
			}
		}

		[HttpGet]
		[Produces(typeof(FitbitWeekOverviewReponse))]
		public async Task<IActionResult> GetWeekOverview(string clientId)
		{
			var activityStepsTask = FitbitHelper.GetResponseList<ActivityStep>(
				_dataContext,
				_fitbitSettings,
				clientId,
				"https://api.fitbit.com/1/user/-/activities/steps/date/today/1w.json",
				"activities-steps");

			var activityDistanceTask = FitbitHelper.GetResponseList<ActivityDistance>(
					_dataContext,
					_fitbitSettings,
					clientId,
					"https://api.fitbit.com/1/user/-/activities/distance/date/today/1w.json",
					"activities-distance");

			await Task.WhenAll(activityStepsTask, activityDistanceTask);

			var fitbitWeekOverviewReponse = new FitbitWeekOverviewReponse();

			foreach (var activityStep in activityStepsTask.Result)
			{
				fitbitWeekOverviewReponse.FitbitWeekOverviewItems.Add(new FitbitWeekOverviewItem
				{
					Date = activityStep.DateTime,
					Steps = activityStep.Value
				});
			}

			foreach (var activityDistance in activityDistanceTask.Result)
			{
				var existingItem = fitbitWeekOverviewReponse.FitbitWeekOverviewItems
					.SingleOrDefault(item => item.Date == activityDistance.DateTime);

				if (existingItem == null)
				{
					fitbitWeekOverviewReponse.FitbitWeekOverviewItems.Add(new FitbitWeekOverviewItem
					{
						Date = activityDistance.DateTime,
						KiloMeters = Math.Round(Decimal.Parse(activityDistance.Value, NumberStyles.Any, CultureInfo.InvariantCulture), 2)
					});
				}
				else
				{
					existingItem.KiloMeters = Math.Round(Decimal.Parse(activityDistance.Value, NumberStyles.Any, CultureInfo.InvariantCulture), 2);
				}
			}

			fitbitWeekOverviewReponse.FitbitWeekOverviewItems = fitbitWeekOverviewReponse.FitbitWeekOverviewItems
				.OrderByDescending(item => item.Date)
				.ToList();

			return Json(fitbitWeekOverviewReponse);
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

	public class FitbitWeekOverviewReponse
	{
		public List<FitbitWeekOverviewItem> FitbitWeekOverviewItems { get; set; } = new List<FitbitWeekOverviewItem>();
	}

	public class FitbitWeekOverviewItem
	{
		public DateTime Date { get; set; }
		public int Steps { get; set; }
		public decimal KiloMeters { get; set; }
	}
}
