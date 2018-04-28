using HouseDB.Api.Data.FitbitResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseDB.Api.Data.Models
{
	public class FitbitActivityStep : SqlBase
    {
		[JsonIgnore]
		public FitbitClientDetail FitbitClientDetail { get; set; }
		public int Steps { get; set; }
		public DateTime Date { get; set; }

		public static List<FitbitActivityStep> GetFitbitActivitySteps(List<ActivityStep> activitySteps, FitbitClientDetail fitbitClientDetail)
		{
			var fitbitActivitySteps = new List<FitbitActivityStep>();

			if (activitySteps == null || !activitySteps.Any())
			{
				return fitbitActivitySteps;
			}

			foreach (var activityStep in activitySteps)
			{
				fitbitActivitySteps.Add(new FitbitActivityStep
				{
					FitbitClientDetail = fitbitClientDetail,
					Date = activityStep.DateTime,
					Steps = activityStep.Value
				});
			}

			return fitbitActivitySteps;
		}
	}
}
