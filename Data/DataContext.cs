﻿using Microsoft.EntityFrameworkCore;
using HouseDB.Data.Models;

namespace HouseDB.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<ConfigurationValue> ConfigurationValues { get; set; }
		public DbSet<HeaterMeterGroup> HeaterMeterGroups { get; set; }
		public DbSet<HeaterMeter> HeaterMeters { get; set; }
		public DbSet<HeaterValue> HeaterValues { get; set; }
	}
}
