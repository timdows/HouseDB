using Exporter.HouseDBService;
using Exporter.Models.Settings;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Exporter
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

			var houseDBSettings = await GetHouseDBSettings();
			services.AddSingleton(houseDBSettings);

			var jwtToken = await GetJWTAccessToken(houseDBSettings);

			var svcClientCreds = new TokenCredentials(jwtToken, "Bearer");

			// Get other settings via API
			using (var client = new HouseDBAPI(new Uri(houseDBSettings.ApiUrl)))
			{
				var domoticzSettings = await client.SettingsGetDomoticzSettingsGetAsync();
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

		private static async Task<string> GetJWTAccessToken(HouseDBSettings houseDBSettings)
		{
			var disco = await DiscoveryClient.GetAsync(houseDBSettings.IS4Url);

			// request token
			var tokenClient = new TokenClient(disco.TokenEndpoint, houseDBSettings.ClientID, houseDBSettings.Password);
			var tokenResponse = await tokenClient.RequestClientCredentialsAsync(houseDBSettings.Scope);

			return tokenResponse.AccessToken;
		}
	}
}
