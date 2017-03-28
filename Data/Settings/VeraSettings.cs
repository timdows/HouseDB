namespace HouseDB.Data.Settings
{
	public class VeraSettings
    {
		public string VeraIpAddress { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string DataMineDirectoryPath { get; set; } // DataMine database on the vera3
		public string ExportPathOnPi { get; set; } // Working path on the Raspberry Pi
	}
}
