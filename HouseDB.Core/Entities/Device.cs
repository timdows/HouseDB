﻿namespace HouseDB.Core.Entities
{
	public class Device : SqlBase
	{
		public string Name { get; set; }
		public int DataMineChannel { get; set; }
		public int VeraChannel { get; set; }

		public int DomoticzKwhIdx { get; set; }
		public int DomoticzWattIdx { get; set; }

		public bool IsForKwhImport { get; set; }
		public bool IsForTemperatureImport { get; set; }

		public int DomoticzMotionDetectionIdx { get; set; }
	}
}
