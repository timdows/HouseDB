namespace HouseDB.Data.Models
{
	public class Device : SqlBase
    {
		public string Name { get; set; }
		public int DataMineChannel { get; set; }
		public int VeraChannel { get; set; }
		public bool IsForKwhImport { get; set; }
		public bool IsForTemperatureImport { get; set; }
	}
}
