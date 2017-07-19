using HouseDB.Controllers.SevenSegment;
using HouseDB.Data;
using HouseDB.Data.Exporter;
using HouseDB.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

		public void InsertCurrentPowerValues([FromBody] ExporterCurrentPowerValues exporterCurrentPowerValues)
		{
			_memoryCache.Set(nameof(ExporterCurrentPowerValues), exporterCurrentPowerValues);
		}

		public void InsertDomoticzP1Consumption([FromBody] List<DomoticzP1Consumption> domoticzP1Consumptions)
		{
			_memoryCache.Set(nameof(List<DomoticzP1Consumption>), domoticzP1Consumptions);
		}

		public async Task<JsonResult> UploadDatabase(ICollection<IFormFile> files)
		{
			Log.Debug("ExporterController files count {0}", files.Count);

			foreach (var file in files)
			{
				Log.Warning(file.FileName);
				if (file.Length > 0)
				{
					// Save the information to database
					var dateTime = DateTime.Now;
					var exportFile = new ExportFile
					{
						ContentType = file.ContentType,
						Length = file.Length,
						DateAdded = dateTime,
						OriginalFileName = file.FileName,
						FileName = $"{dateTime.ToString("yyyyMMdd-HHmmss")}-{file.FileName}"
					};

					await _dataContext.AddAsync(exportFile);
					await _dataContext.SaveChangesAsync();

					// Save file to disk
					var exportPath = Path.Combine(Directory.GetCurrentDirectory(), "exports");
					if (!Directory.Exists(exportPath))
					{
						Directory.CreateDirectory(exportPath);
					}

					var diskFileName = Path.Combine(exportPath, file.FileName);
					using (var fileStream = new FileStream(diskFileName, FileMode.Create))
					{
						await file.CopyToAsync(fileStream);
					}
				}
			}

			return Json(true);
		}
	}
}
