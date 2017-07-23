﻿using HouseDB.Data;
using HouseDB.Data.Statistics;
using HouseDB.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HouseDB.Controllers.Statistics
{
	[Route("[controller]/[action]")]
	public class StatisticsController : HouseDBController
	{
		public StatisticsController(DataContext dataContext) : base(dataContext)
		{
		}

		[HttpPost]
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
			for (DateTime day = new DateTime(postGetKwhYearUsage.Year, 1, 1); day <= new DateTime(postGetKwhYearUsage.Year, 12, 31); day = day.AddDays(1))
			{
				var dayUsage = kwhDateUsages.SingleOrDefault(a_item => a_item.Date == day)?.Usage;

				clientModel.DayValues.Add(new KwhDayUsageValue
				{
					Date = day,
					Usage = dayUsage.GetValueOrDefault()
				});
			}

			return Json(clientModel);
		}
	}
}
