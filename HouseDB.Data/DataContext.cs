using HouseDB.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HouseDB.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
		public DbSet<P1Consumption> P1Consumptions { get; set; }

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
			optionsBuilder.UseMySql(config.GetConnectionString("HouseDBDatabase"));

			return new DataContext(optionsBuilder.Options);
		}
	}
}
