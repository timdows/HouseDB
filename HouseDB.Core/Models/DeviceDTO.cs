namespace HouseDB.Core.Models
{
	public class DeviceDTO
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int DomoticzKwhIdx { get; set; }
		public int DomoticzWattIdx { get; set; }

		public bool IsForKwhImport { get; set; }
		public bool IsForTemperatureImport { get; set; }

		public int DomoticzMotionDetectionIdx { get; set; }
	}
}
