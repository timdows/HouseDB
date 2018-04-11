using HouseDB.Api.Data.FitbitResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HouseDB.Api.Data.Models
{
	public class FitbitActivityDistance : SqlBase
    {
		[JsonIgnore]
		public FitbitClientDetail FitbitClientDetail { get; set; }
		public DateTime Date { get; set; }
		public decimal KiloMeters { get; set; }

		public static List<FitbitActivityDistance> GetFitbitActivityDistances(List<ActivityDistance> activityDistances, FitbitClientDetail fitbitClientDetail)
		{
			var fitbitActivityDistances = new List<FitbitActivityDistance>();

			foreach (var activityDistance in activityDistances)
			{
				fitbitActivityDistances.Add(new FitbitActivityDistance
				{
					FitbitClientDetail = fitbitClientDetail,
					Date = activityDistance.DateTime,
					KiloMeters = Decimal.Parse(activityDistance.Value, NumberStyles.Any, CultureInfo.InvariantCulture)
				});
			}

			return fitbitActivityDistances;
		}
	}
}
