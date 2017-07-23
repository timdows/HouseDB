using HouseDB.Data;
using Microsoft.AspNetCore.Mvc;
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
		public JsonResult GetDevice([FromQuery] long deviceID)
		{
			var device = _dataContext.Devices
				.SingleOrDefault(a_item => a_item.ID == deviceID);

			return Json(device);
		}

		[HttpGet]
		[Produces(typeof(Data.Models.Device))]
		public JsonResult GetAllKwhExportDevices()
		{
			var devices = _dataContext.Devices
				.Where(a_item => a_item.IsForKwhImport &&
								 a_item.DomoticzIdx != 0)
				.ToList();

			return Json(devices);
		}
	}
}
