using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace MysensorElectronDB.Data
{
	public class EFConfigProvider : ConfigurationProvider
	{
		public EFConfigProvider(Action<DbContextOptionsBuilder> optionsAction)
		{
			OptionsAction = optionsAction;
		}

		Action<DbContextOptionsBuilder> OptionsAction { get; }

		public override void Load()
		{
			var builder = new DbContextOptionsBuilder<DataContext>();
			OptionsAction(builder);

			using (var dataContext = new DataContext(builder.Options))
			{
				//dataContext.Database.EnsureCreated();
				dataContext.Database.Migrate();
				Data = dataContext.ConfigurationValues.ToDictionary(c => c.Setting, c => c.Value);
			}
		}
	}
}
