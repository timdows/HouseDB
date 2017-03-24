﻿using System;

namespace HouseDB.Data.Models
{
	public class ExportFile : SqlBase
    {
		public DateTime DateAdded { get; set; }
		public string FileName { get; set; }
		public string ContentType { get; set; }
		public long Length { get; set; }
	}
}
