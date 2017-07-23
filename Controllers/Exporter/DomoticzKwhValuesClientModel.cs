using System.Collections.Generic;

namespace HouseDB.Controllers.Exporter
{
	public class DomoticzKwhValuesClientModel
    {
		public Data.Models.Device Device { get; set; }
		public List<Data.Exporter.DomoticzKwhUsage> DomoticzKwhUsages { get; set; }
	}
}
