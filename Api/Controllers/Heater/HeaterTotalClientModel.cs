using HouseDB.Data;
using HouseDB.Data.Heater;
using HouseDB.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HouseDB.Controllers.Heater
{
	public class HeaterTotalClientModel : BaseClientModel
	{
		public HeaterMeterGroup HeaterMeterGroup { get; set; } = new HeaterMeterGroup();
		public List<HeaterGraphData> YearGraphData { get; set; } = new List<HeaterGraphData>();

		public void Load(DataContext dataContext, List<List<HeaterClientValue>> heaterClientValues)
		{
			HeaterMeterGroup.Name = "Total";

			for (int year = 2013; year <= DateTime.Today.Year; year++)
			{
				var values = new List<HeaterGraphValue>();

				for (int month = 1; month <= 12; month++)
				{
					var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);

					var sum = 0;
					foreach (var heaterClientValue in heaterClientValues)
					{
						sum += heaterClientValue
							.Where(a_item => a_item.Date.Month == month && a_item.Date.Year == year)
							.Sum(a_item => a_item.DifferenceValue);
					}

					values.Add(new HeaterGraphValue
					{
						Label = monthName,
						Value = sum
					});
				}

				YearGraphData.Add(new HeaterGraphData
				{
					Key = year.ToString(),
					Values = values
				});
			}
		}
	}
}
