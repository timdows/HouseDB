using HouseDB.Data;
using HouseDB.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Controllers.Heater
{
	public class HeaterClientModel
	{
		public HeaterMeterGroup HeaterMeterGroup { get; set; }
		public List<HeaterValue> HeaterValues { get; set; }

		public void Load(DataContext dataContext, long heaterMeterGroupID)
		{
			HeaterMeterGroup = dataContext.HeaterMeterGroups
				.SingleOrDefault(a_item => a_item.ID == heaterMeterGroupID);

			if (HeaterMeterGroup == null)
			{
				return;
			}

			HeaterValues = dataContext.HeaterValues
				.Include(a_item => a_item.HeaterMeter.HeaterMeterGroup)
				.Where(a_item => a_item.HeaterMeter.HeaterMeterGroup.ID == HeaterMeterGroup.ID)
				.OrderBy(a_item => a_item.Date)
				.ToList();
		}
	}

	//public class HeaterClientValue
	//{
	//	public DateTime Date { get; set; }
	//	public int Value { get; set; }

	//}
}
