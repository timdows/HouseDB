using HouseDB.Core;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HouseDB.Fitbit
{
	public class Startup
    {
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
        {
			// Create singleton of jwtTokenManager instance
			var jwtTokenManager = new JwtTokenManager();
			services.AddSingleton(jwtTokenManager);

			services.Configure<FitbitSettings>(Configuration.GetSection("FitbitSettings"));
			services.Configure<HouseDBSettings>(Configuration.GetSection("HouseDBSettings"));

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
	}
}
