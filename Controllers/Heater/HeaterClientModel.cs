using HouseDB.Data;
using HouseDB.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Controllers.Heater
{
	public class HeaterClientModel
	{
		public HeaterMeterGroup HeaterMeterGroup { get; set; }
		public List<HeaterValue> HeaterValues { get; set; }
		public List<HeaterClientValue> HeaterClientValues { get; set; } = new List<HeaterClientValue>();

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

			var sum = 0;
			var lastValue = 0;
			foreach(var heaterValue in HeaterValues)
			{
				var difference = 0;
				if (lastValue != 0 && heaterValue.Value >= lastValue)
				{
					difference = heaterValue.Value - lastValue;
					lastValue = heaterValue.Value;
				}
				else
				{
					difference = heaterValue.Value;
					lastValue = heaterValue.Value;
				}

				sum += difference;

				HeaterClientValues.Add(new HeaterClientValue
				{
					Date = heaterValue.Date,
					PureValue = heaterValue.Value,
					SumValue = sum,
					DifferenceValue = difference
				});
			}
		}
	}

	public class HeaterClientValue
	{
		public DateTime Date { get; set; }
		public int PureValue { get; set; }
		public int SumValue { get; set; }
		public int DifferenceValue { get; set; }
	}
}
