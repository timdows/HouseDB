using System.Collections.Generic;

namespace HouseDB.Api.Data.Heater
{
	public class HeaterGraphData
	{
		public List<HeaterGraphValue> Values { get; set; } = new List<HeaterGraphValue>();
		public string Key { get; set; }
	}
}
