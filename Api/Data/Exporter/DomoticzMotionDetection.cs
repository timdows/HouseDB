using System;

namespace HouseDB.Data.Exporter
{
	public class DomoticzMotionDetection
    {
		public string Data { get; set; }
		public DateTime Date { get; set; }
		public int Level { get; set; }
		public string Status { get; set; }
	}
}
