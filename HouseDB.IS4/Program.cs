using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Net;

namespace HouseDB.IS4
{
	public class Program
    {
        public static void Main(string[] args)
        {
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			try
			{
				Log.Information("Getting HouseDB/IS4 running...");

				var host = new WebHostBuilder()
					.UseKestrel(options =>
					{
						options.Listen(IPAddress.Loopback, 5010);
					})
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseStartup<Startup>()
					.UseConfiguration(configuration)
					.UseSerilog()
					.Build();

				host.Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Host HouseDB/IS4 terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}
    }
}
