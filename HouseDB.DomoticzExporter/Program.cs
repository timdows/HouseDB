using HouseDB.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Net.Http.Headers;
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
			//var dabSettings = config.GetSection("HouseDBSettings").Get<HouseDBSettings>();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();
			Log.Information($"Starting HouseDB.DomoticzExporter {Assembly.GetExecutingAssembly().GetName().Version}");

			// Support typed Options
			services.AddOptions();
			services.SetupDI(config);

			//// Create singleton of jwtTokenManager instance
			//var jwtTokenManager = new JwtTokenManager();
			//services.AddSingleton(jwtTokenManager);

			//Log.Information("Getting HouseDBSettings from appsettings.json");
			//var houseDBSettings = await GetHouseDBSettings();
			//services.AddSingleton(houseDBSettings);

			//Log.Information("Getting JWTAccessToken");
			//var jwtToken = await jwtTokenManager.GetToken(houseDBSettings);

			//Log.Information("Getting other settings via API");
			//using (var api = new HouseDBAPI(new Uri(houseDBSettings.ApiUrl)))
			//{
			//	api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
			//	var domoticzSettings = await api.SettingsGetDomoticzSettingsGetAsync();
			//	services.AddSingleton(domoticzSettings);
			//}

			services.AddTransient<Application>();
		}
	}
}
