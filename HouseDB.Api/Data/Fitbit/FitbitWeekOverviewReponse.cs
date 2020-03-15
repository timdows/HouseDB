using System.Collections.Generic;

namespace HouseDB.Api.Data.Fitbit
{
	public class FitbitWeekOverviewReponse
	{
		public List<FitbitWeekOverviewItem> FitbitWeekOverviewItems { get; set; } = new List<FitbitWeekOverviewItem>();
	}
}
