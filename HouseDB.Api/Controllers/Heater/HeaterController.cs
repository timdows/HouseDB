﻿using HouseDB.Api.Data;
using HouseDB.Api.Data.Heater;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Api.Controllers.Heater
{
	[Route("[controller]/[action]")]
	public class HeaterController : HouseDBController
	{
		public HeaterController(DataContext dataContext) : base(dataContext)
		{
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var meters = _dataContext.HeaterMeters
				.Include(a_item => a_item.HeaterValues)
				.ToList();

			return Json(meters);
		}

		[HttpGet]
		public IActionResult GetClientModel()
		{
			var groups = _dataContext.HeaterMeterGroups.ToList();
			var clientModels = new List<HeaterGroupClientModel>();
			var heaterClientValues = new List<List<HeaterClientValue>>();

			foreach (var group in groups)
			{
				var clientModel = new HeaterGroupClientModel();
				clientModel.Load(_dataContext, group.ID);

				clientModels.Add(clientModel);
				heaterClientValues.Add(clientModel.HeaterClientValues);
			}

			var total = new HeaterTotalClientModel();
			total.Load(_dataContext, heaterClientValues);

			return Json(new { total, clientModels });
		}
	}
}