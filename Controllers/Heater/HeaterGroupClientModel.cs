using HouseDB.Data;
using HouseDB.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HouseDB.Controllers.Heater
{
	public class HeaterGroupClientModel
	{
		public HeaterMeterGroup HeaterMeterGroup { get; set; }
		public List<HeaterGraphData> YearGraphData { get; set; } = new List<HeaterGraphData>();
		public List<HeaterClientValue> HeaterClientValues { get; set; } = new List<HeaterClientValue>();

		public void Load(DataContext dataContext, long heaterMeterGroupID)
		{
			HeaterMeterGroup = dataContext.HeaterMeterGroups
				.SingleOrDefault(a_item => a_item.ID == heaterMeterGroupID);

			if (HeaterMeterGroup == null)
			{
				return;
			}

			HeaterClientValues = GetHeaterClientValues(dataContext, HeaterMeterGroup.ID);

			var years = HeaterClientValues.GroupBy(a_item => a_item.Date.Year, a_item => a_item);

			foreach (var year in years)
			{
				var values = new List<HeaterGraphValue>();
				for (int i = 1; i <= 12; i++)
				{
					var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i);
					var value = year.Where(a_item => a_item.Date.Month == i).Sum(a_item => a_item.DifferenceValue);

					values.Add(new HeaterGraphValue
					{
						Label = monthName,
						Value = value
					});
				}

				YearGraphData.Add(new HeaterGraphData
				{
					Key = year.Key.ToString(),
					Values = values
				});
			}
		}

		private static List<HeaterClientValue> GetHeaterClientValues(DataContext dataContext, long heaterMeterGroupID)
		{
			var HeaterClientValues = new List<HeaterClientValue>();

			var heaterValues = dataContext.HeaterValues
							.Include(a_item => a_item.HeaterMeter.HeaterMeterGroup)
							.Where(a_item => a_item.HeaterMeter.HeaterMeterGroup.ID == heaterMeterGroupID)
							.OrderBy(a_item => a_item.Date)
							.ToList();

			var sum = 0;
			var lastValue = 0;
			foreach (var heaterValue in heaterValues)
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

			return HeaterClientValues;
		}
	}
}
