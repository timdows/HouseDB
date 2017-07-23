using System;

namespace HouseDB.Data.Exporter
{
	public class DomoticzKwhUsage
    {
		public decimal C { get; set; }
		public DateTime D { get; set; }
		public decimal V { get; set; }

		public decimal Usage => V;
		public DateTime Date => D;
	}
}
