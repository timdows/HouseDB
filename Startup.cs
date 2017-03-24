﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HouseDB.Data;
using MySQL.Data.Entity.Extensions;
using Newtonsoft.Json;
using Serilog;
using HouseDB.Settings;

namespace HouseDB
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.Prod.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();

			builder.AddEntityFrameworkConfig(options => options.UseMySQL(Configuration["Database:ConnectionString"]));
			Configuration = builder.Build();

			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.LiterateConsole()
				.ReadFrom.Configuration(Configuration)
				.CreateLogger();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().AddJsonOptions(options => {
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			});

			services.AddCors();

			var connection = Configuration["Database:ConnectionString"];
			services.AddDbContext<DataContext>(options => options.UseMySQL(connection));
			services.Configure<VeraSettings>(Configuration.GetSection("VeraSettings"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
		{
			loggerFactory.AddSerilog();
			appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

			app.UseCors(builder => builder.AllowAnyOrigin());

			app.UseMvc(routes =>
			{
				routes.MapRoute(name: "jsonRoute", template: "{controller}/{action}.json");
				routes.MapRoute(name: "postRoute", template: "{controller}/{action}");
			});
		}
	}
}
