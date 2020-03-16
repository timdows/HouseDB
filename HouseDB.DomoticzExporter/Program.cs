using HouseDB.Core.SettingModels;
using HouseDB.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace HouseDB.DomoticzExporter
{
	public class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection).Wait();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var app = serviceProvider.GetService<Application>();
            Task.Run(() => app.Run()).Wait();
        }

		private static async Task ConfigureServices(IServiceCollection services)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", true)
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();
			Log.Information($"Starting HouseDB.DomoticzExporter {Assembly.GetExecutingAssembly().GetName().Version}");
			
			services.SetupDI(config);

			services.Configure<DomoticzSettings>(config.GetSection("DomoticzSettings"));
			services.AddTransient<Application>();
		}
	}
}
