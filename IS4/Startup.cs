using IS4.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace IS4
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
			var identityServerSettings = Configuration.GetSection("IdentityServerSettings").Get<IdentityServerSettings>();
			var cert = new X509Certificate2(
				Path.Combine(Directory.GetCurrentDirectory(), identityServerSettings.CertificateSettings.FileName), 
				identityServerSettings.CertificateSettings.Password);

			services.AddIdentityServer()
				.AddSigningCredential(cert)
				.AddInMemoryApiResources(Config.GetApiResources(identityServerSettings.IdentityServerApis))
				.AddInMemoryClients(Config.GetClients(identityServerSettings.IdentityServerApis));
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			var fordwardedHeaderOptions = new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			};
			fordwardedHeaderOptions.KnownNetworks.Clear();
			fordwardedHeaderOptions.KnownProxies.Clear();

			app.UseForwardedHeaders(fordwardedHeaderOptions);

			app.Map("/is4", api =>
			{
				api.UseIdentityServer();
			});
		}
    }
}
