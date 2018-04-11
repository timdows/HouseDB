using HouseDB.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HouseDB.Api.Data
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
		public DbSet<MotionDetection> MotionDetections { get; set; }
		public DbSet<P1Consumption> P1Consumptions { get; set; }

		public DbSet<FitbitClientDetail> FitbitClientDetails { get; set; }
		public DbSet<FitbitAuthCode> FitbitAuthCodes { get; set; }
		public DbSet<FitbitAccessToken> FitbitAccessTokens { get; set; }
		public DbSet<FitbitActivityStep> FitbitActivitySteps { get; set; }
		public DbSet<FitbitActivityDistance> FitbitActivityDistances { get; set; }
	}

	public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
	{
		public DataContext CreateDbContext(string[] args)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			var config = builder.Build();

			var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
			optionsBuilder.UseMySql(config["Database:ConnectionString"]);

			return new DataContext(optionsBuilder.Options);
		}
	}
}
