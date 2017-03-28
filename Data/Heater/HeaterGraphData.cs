using System.Collections.Generic;

namespace HouseDB.Data.Heater
{
	public class HeaterGraphData
	{
		public List<HeaterGraphValue> Values { get; set; } = new List<HeaterGraphValue>();
		public string Key { get; set; }
	}
}
