using HouseDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HouseDB.Controllers.Heater
{
	public class HeaterController : HouseDBController
	{
		public HeaterController(DataContext dataContext) : base(dataContext)
		{
		}

		public IActionResult GetAll()
		{
			var meters = _dataContext.HeaterMeters
				.Include(a_item => a_item.HeaterValues)
				.ToList();

			return Json(meters);
		}

		public IActionResult GetClientModel([FromQuery] long heaterMeterGroupID)
		{
			var clientModel = new HeaterClientModel();
			clientModel.Load(_dataContext, heaterMeterGroupID);
			return Json(clientModel);
		}
	}
}
