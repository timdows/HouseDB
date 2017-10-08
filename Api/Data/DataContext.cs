using HouseDB.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
		public DbSet<Device> Devices { get; set; }
		public DbSet<ExportFile> ExportFiles { get; set; }
		public DbSet<KwhDeviceValue> KwhDeviceValues { get; set; }
		public DbSet<KwhDateUsage> KwhDateUsages { get; set; }
		public DbSet<ExpenseType> ExpenseTypes { get; set; }
		public DbSet<ExpenseRecord> ExpenseRecords { get; set; }
		public DbSet<MotionDetection> MotionDetections { get; set; }
	}

	public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
	{
		public DataContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			optionsBuilder.UseMySql("");

			return new DataContext(optionsBuilder.Options);
		}
	}
}
