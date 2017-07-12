using HouseDB.Controllers.SevenSegment;
using HouseDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace HouseDB.Controllers.Exporter
{
	public class ExporterController : HouseDBController
	{
		private readonly IMemoryCache _memoryCache;

		public ExporterController(DataContext dataContext, IMemoryCache memoryCache) : base(dataContext)
		{
			_memoryCache = memoryCache;
		}

		public void InsertCurrentWattValue([FromBody] int wattValue)
		{
			var clientModel = new WattValueClientModel
			{
				Watt = wattValue,
				DateTimeAdded = DateTime.Now
			};
			_memoryCache.Set($"{nameof(ExporterController)}_WattValue", clientModel);
		}
	}
}
