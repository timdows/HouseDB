﻿namespace HouseDB.Data.Models
{
	public class ConfigurationValue : SqlBase
	{
		public string Setting { get; set; }
		public string Value { get; set; }
	}
}