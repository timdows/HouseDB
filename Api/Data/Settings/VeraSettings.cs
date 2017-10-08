namespace HouseDB.Data.Settings
{
	public class VeraSettings
    {
		public string VeraIpAddress { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string DataMineDirectoryPath { get; set; } // DataMine database on the vera3
		public string ExportPathOnPi { get; set; } // Working path on the Raspberry Pi
		public int WattChannel { get; set; } // Channel to get the current Watt from smart meter from
	}
}
