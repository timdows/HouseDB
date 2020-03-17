using AutoMapper;
using HouseDB.Core.Interfaces;
using HouseDB.Data;
using HouseDB.Data.MemoryCaches;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HouseDB.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void SetupDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseMySql(configuration.GetConnectionString("HouseDBDatabase")));
            services.AddMediatR(typeof(Core.Entities.SqlBase).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(Core.Entities.SqlBase).GetTypeInfo().Assembly);

            services.AddSingleton<IDomoticzMemoryCache, DomoticzMemoryCache>();

            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddTransient<IP1ConsumptionRepository, P1ConsumptionRepository>();
            services.AddTransient<IKwhDateUsageRepository, KwhDateUsageRepository>();
        }
    }
}
