using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HouseDB.Core.SettingModels;
using HouseDB.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HouseDB.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
			//CreateHostBuilder(args).Build().Run();
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", true)
				.Build();
			var dabSettings = config.GetSection("HouseDBSettings").Get<HouseDBSettings>();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();
			Log.Information($"Starting DAB web host on port {dabSettings.HostingEnvironmentPort}");

			try
			{
				var host = CreateHostBuilder(dabSettings.HostingEnvironmentPort, args).Build();

				using (var scope = host.Services.CreateScope())
				{
					var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
					await dataContext.Database.MigrateAsync();
				}

				await host.RunAsync();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Host terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

        public static IHostBuilder CreateHostBuilder(int port, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Loopback, port);
                    });
                });
    }
}
