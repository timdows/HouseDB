﻿namespace HouseDB.Data.Settings
{
	public class DomoticzSettings
    {
		public string Address { get; set; }
		public int WattIdx { get; set; } // IDX to get the current Watt from smart meter from
	}
}