using HouseDB.Core;
using HouseDB.Core.Settings;
using HouseDB.Services.Api;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace HouseDB.Exporter
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
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.Enrich.FromLogContext()
				.WriteTo.LiterateConsole()
				.WriteTo.RollingFile("logs/log-{Date}.txt")
				.CreateLogger();

			// Support typed Options
			services.AddOptions();

			Log.Information($"-----Configure services for Exporter {Assembly.GetExecutingAssembly().GetName().Version}");

			// Create singleton of jwtTokenManager instance
			var jwtTokenManager = new JwtTokenManager();
			services.AddSingleton(jwtTokenManager);

			Log.Information("Getting HouseDBSettings from appsettings.json");
			var houseDBSettings = await GetHouseDBSettings();
			services.AddSingleton(houseDBSettings);

			Log.Information("Getting JWTAccessToken");
			var jwtToken = await jwtTokenManager.GetToken(houseDBSettings);

			Log.Information("Getting other settings via API");
			using (var api = new HouseDBAPI(new Uri(houseDBSettings.ApiUrl)))
			{
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
				var domoticzSettings = await api.SettingsGetDomoticzSettingsGetAsync();
				services.AddSingleton(domoticzSettings);
			}
			
			services.AddTransient<Application>();
		}

		private static async Task<HouseDBSettings> GetHouseDBSettings()
		{
			// Get settings from appconfig.json
			var appsettingsString = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
			var appsettings = JsonConvert.DeserializeObject<dynamic>(appsettingsString);
			return JsonConvert.DeserializeObject<HouseDBSettings>(appsettings.HouseDBSettings.ToString());
		}
	}
}
