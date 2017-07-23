using HouseDB.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Controllers.Device
{
	[Route("[controller]/[action]")]
	public class DeviceController : HouseDBController
	{
		public DeviceController(DataContext dataContext) : base(dataContext)
		{
		}

		[HttpGet]
		[Produces(typeof(Data.Models.Device))]
		public JsonResult GetDevice([FromQuery] long deviceID)
		{
			var device = _dataContext.Devices
				.SingleOrDefault(a_item => a_item.ID == deviceID);

			return Json(device);
		}

		[HttpGet]
		[Produces(typeof(List<Data.Models.Device>))]
		public JsonResult GetAllKwhExportDevices()
		{
			var devices = _dataContext.Devices
				.Where(a_item => a_item.IsForKwhImport &&
								 a_item.DomoticzKwhIdx != 0)
				.ToList();

			return Json(devices);
		}

		[HttpGet]
		[Produces(typeof(List<Data.Models.Device>))]
		public JsonResult GetAllDevicesForCachingValues()
		{
			var devices = _dataContext.Devices
				.Where(a_item => a_item.IsForKwhImport &&
								 (a_item.DomoticzWattIdx != 0 || a_item.DomoticzKwhIdx != 0))
				.ToList();

			return Json(devices);
		}
	}
}
