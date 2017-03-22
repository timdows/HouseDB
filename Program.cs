using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MysensorElectronDB
{
	public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
				.UseUrls("http://localhost:5002")
				.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
