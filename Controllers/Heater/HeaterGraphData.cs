using System.Collections.Generic;

namespace HouseDB.Controllers.Heater
{
	public class HeaterGraphData
	{
		public List<HeaterGraphValue> Values { get; set; } = new List<HeaterGraphValue>();
		public string Key { get; set; }
	}
}
