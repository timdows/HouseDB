using Newtonsoft.Json.Linq;

namespace HouseDB.Data.DataMine
{
	public class DataMineDoubleReturnValue
    {
		public JObject Json { get; set; }
		public string Url { get; set; }
		public decimal HighStart { get; set; }
		public decimal HighEnd { get; set; }
		public decimal LowStart { get; set; }
		public decimal LowEnd { get; set; }
	}
}
