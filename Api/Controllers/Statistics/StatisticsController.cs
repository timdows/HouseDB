using Api.Controllers.Statistics;
using HouseDB.Data;
using HouseDB.Data.Statistics;
using HouseDB.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace HouseDB.Controllers.Statistics
{
	[Route("[controller]/[action]")]
	public class StatisticsController : HouseDBController
	{
		private readonly IMemoryCache _memoryCache;

		public StatisticsController(
			DataContext dataContext,
			IMemoryCache memoryCache) : base(dataContext)
		{
			_memoryCache = memoryCache;
		}

		[HttpPost]
		[Produces(typeof(KwhYearUsageClientModel))]
		public JsonResult GetKwhYearUsage([FromBody] PostGetKwhYearUsage postGetKwhYearUsage)
		{
			var device = _dataContext.Devices.Single(a_item => a_item.ID == postGetKwhYearUsage.DeviceID);
			var kwhDateUsages = _dataContext.KwhDateUsages
				.Where(a_item => a_item.DeviceID == postGetKwhYearUsage.DeviceID &&
								 a_item.Date.Year == postGetKwhYearUsage.Year)
				.ToList();

			var clientModel = new KwhYearUsageClientModel
			{
				Device = device,
				Year = postGetKwhYearUsage.Year,
				YearUsage = kwhDateUsages.Sum(a_item => a_item.Usage)
			};

			// Fill the months
			for (var i = 1; i <= 12; i++)
			{
				var monthUsage = kwhDateUsages
					.Where(a_item => a_item.Date.Month == i)
					.Sum(a_item => a_item.Usage);
				clientModel.MonthValues.Add(new KwhMonthUsageValue
				{
					Month = i,
					Usage = monthUsage
				});
			}

			// Fill the weeks
			for (var i = 1; i < DateTimeExtension.GetWeeksInYear(postGetKwhYearUsage.Year); i++)
			{
				var weekUsage = kwhDateUsages
					.Where(a_item => a_item.Date.GetIso8601WeekOfYear() == i)
					.Sum(a_item => a_item.Usage);
				clientModel.WeekValues.Add(new KwhWeekUsageValue
				{
					Week = i,
					Usage = weekUsage
				});
			}

			// Fill the days
			var dayValues = kwhDateUsages
				.Where(a_item => a_item.Date >= new DateTime(postGetKwhYearUsage.Year, 1, 1) &&
								 a_item.Date <= new DateTime(postGetKwhYearUsage.Year, 12, 31))
				.ToList();
			clientModel.DayValues = dayValues;

			return Json(clientModel);
		}

		[HttpPost]
		public JsonResult GetMotionDetections([FromBody] PostGetMotionDetections postGetMotionDetections)
		{
			return Json(true);
		}

		[HttpGet]
		[Produces(typeof(CurrentUsagesClientModel))]
		public JsonResult GetCurrentUsages()
		{
			var clientModel = new CurrentUsagesClientModel();
			clientModel.Load(_dataContext, _memoryCache);
			return Json(clientModel);
		}
	}
}
