using HouseDB.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HouseDB.Controllers.Device
{
	public class DeviceController : HouseDBController
	{
		public DeviceController(DataContext dataContext) : base(dataContext)
		{
		}

		public JsonResult GetDevice([FromQuery] long deviceID)
		{
			var device = _dataContext.Devices
				.SingleOrDefault(a_item => a_item.ID == deviceID);

			return Json(device);
		}
	}
}
