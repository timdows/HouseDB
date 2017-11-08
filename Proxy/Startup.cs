using HouseDBCore;
using HouseDBCore.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Proxy
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			var houseDBSettings = GetHouseDBSettings().GetAwaiter().GetResult();
			services.AddSingleton(houseDBSettings);

			// Create singleton of jwtTokenManager instance
			var jwtTokenManager = new JwtTokenManager();
			services.AddSingleton(jwtTokenManager);

			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
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
