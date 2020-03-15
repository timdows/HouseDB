using HouseDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace HouseDB.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void SetupDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseMySql(configuration.GetConnectionString("HouseDBDatabase")));
        }
    }
}
