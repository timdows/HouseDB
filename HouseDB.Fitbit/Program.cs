﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace HouseDB.Fitbit
{
	public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseKestrel(options =>
				{
					options.Listen(IPAddress.Loopback, 5012);
				})
				.UseStartup<Startup>()
				.Build();
	}
}
