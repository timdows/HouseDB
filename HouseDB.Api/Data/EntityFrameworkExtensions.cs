using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace HouseDB.Api.Data
{
	public static class EntityFrameworkExtensions
	{
		public static IConfigurationBuilder AddEntityFrameworkConfig(
			this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> setup)
		{
			return builder.Add(new EFConfigSource(setup));
		}
	}
}
