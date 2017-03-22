using Microsoft.EntityFrameworkCore;
using MysensorElectronDB.Data.Models;

namespace MysensorElectronDB.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<ConfigurationValue> ConfigurationValues { get; set; }
	}
}
