using Api.Data.Settings;
using HouseDB.Data;
using HouseDB.Data.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace HouseDB
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();

			builder.AddEntityFrameworkConfig(options => options.UseMySql(Configuration["Database:ConnectionString"]));
			Configuration = builder.Build();

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.Enrich.FromLogContext()
				.WriteTo.LiterateConsole()
				.ReadFrom.Configuration(Configuration)
				.CreateLogger();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc()
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				});

			services.AddCors();

			var connection = Configuration["Database:ConnectionString"];
			services.AddDbContext<DataContext>(options => options.UseMySql(connection));
			services.Configure<VeraSettings>(Configuration.GetSection("VeraSettings"));
			services.Configure<DataMineSettings>(Configuration.GetSection("DataMineSettings"));
			services.Configure<RaspicamSettings>(Configuration.GetSection("RaspicamSettings"));
			services.Configure<DomoticzSettings>(Configuration.GetSection("DomoticzSettings"));
			services.Configure<IdentityServerSettings>(Configuration.GetSection("IdentityServerSettings"));

			// Register the Swagger generator, defining one or more Swagger documents
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Info { Title = "HouseDB API", Version = "v1" });
				options.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
			});

			// Fancypants
			IConfigurationSection sectionData = Configuration.GetSection("IdentityServerSettings");
			var identityServerSettings = new IdentityServerSettings();
			sectionData.Bind(identityServerSettings);

			services.AddAuthentication(options =>
			{
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(options =>
			{
				options.Authority = identityServerSettings.Host;
				options.Audience = identityServerSettings.ApiName;
				options.RequireHttpsMetadata = true;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
		{
			loggerFactory.AddSerilog();
			appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

			app.UseCors(builder => builder.AllowAnyOrigin());

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(name: "jsonRoute", template: "{controller}/{action}.json");
				routes.MapRoute(name: "postRoute", template: "{controller}/{action}");
			});

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger(options =>
			{
				options.RouteTemplate = "swagger/{documentName}/swagger.json";
			});

			// Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "HouseDB API v1");
				options.RoutePrefix = "swagger";
			});
		}
	}
}
