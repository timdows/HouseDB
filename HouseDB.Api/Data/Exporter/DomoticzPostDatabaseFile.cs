using System;

namespace HouseDB.Api.Data.Exporter
{
	public class DomoticzPostDatabaseFile
    {
		public byte[] ByteArray { get; set; }
		public DateTime DateTime { get; set; }
		public string FileName { get; set; }
	}
}
