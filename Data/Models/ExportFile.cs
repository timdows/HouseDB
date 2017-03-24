using System;

namespace HouseDB.Data.Models
{
	public class ExportFile : SqlBase
    {
		public DateTime DateAdded { get; set; }
		public string Filename { get; set; }
		public byte[] Content { get; set; }
	}
}
