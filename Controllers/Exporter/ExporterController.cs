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
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Controllers.Exporter
{
	[Route("[controller]/[action]")]
	public class ExporterController : HouseDBController
	{
		private readonly IMemoryCache _memoryCache;

		public ExporterController(DataContext dataContext, IMemoryCache memoryCache) : base(dataContext)
		{
			_memoryCache = memoryCache;
		}

		[HttpPost]
		public void InsertDomoticzP1Consumption([FromBody] List<DomoticzP1Consumption> domoticzP1Consumptions)
		{
			_memoryCache.Set(nameof(List<DomoticzP1Consumption>), domoticzP1Consumptions);
		}

		[HttpPost]
		public async Task InsertDomoticzKwhValues([FromBody] DomoticzKwhValuesClientModel clientModel)
		{
			// Get the existing values from this device between the clientModel dates
			var existing = _dataContext.KwhDateUsages
				.Where(a_item => a_item.DeviceID == clientModel.Device.ID &&
								 a_item.Date >= clientModel.DomoticzKwhUsages.Min(b_item => b_item.Date) &&
								 a_item.Date <= clientModel.DomoticzKwhUsages.Max(b_item => b_item.Date))
				.ToList();

			foreach (var domoticzKwhUsage in clientModel.DomoticzKwhUsages)
			{
				// Skip value if it is already in the database or if it is today
				if (existing.Any(a_item => a_item.Date == domoticzKwhUsage.Date) || domoticzKwhUsage.Date.Date == DateTime.Today)
				{
					continue;
				}

				_dataContext.KwhDateUsages.Add(new KwhDateUsage
				{
					DeviceID = clientModel.Device.ID,
					Date = domoticzKwhUsage.Date,
					Usage = domoticzKwhUsage.Usage
				});
			}

			await _dataContext.SaveChangesAsync();
		}

		[HttpPost]
		public void InsertValuesForCaching([FromBody] DomoticzValuesForCachingClientModel clientModel)
		{
			_memoryCache.Set(nameof(DomoticzValuesForCachingClientModel), clientModel);
		}

		[HttpPost]
		public async Task UploadDatabase([FromBody] ExportFile exportFile)
		{
			var a = exportFile;

			//Log.Debug("ExporterController files count {0}", files.Count);

			//foreach (var file in files)
			//{
			//	Log.Warning(file.FileName);
			//	if (file.Length > 0)
			//	{
			//		// Save the information to database
			//		var dateTime = DateTime.Now;
			//		var exportFile = new ExportFile
			//		{
			//			ContentType = file.ContentType,
			//			Length = file.Length,
			//			DateAdded = dateTime,
			//			OriginalFileName = file.FileName,
			//			FileName = $"{dateTime.ToString("yyyyMMdd-HHmmss")}-{file.FileName}"
			//		};

			//		await _dataContext.AddAsync(exportFile);
			//		await _dataContext.SaveChangesAsync();

			//		// Save file to disk
			//		var exportPath = Path.Combine(Directory.GetCurrentDirectory(), "exports");
			//		if (!Directory.Exists(exportPath))
			//		{
			//			Directory.CreateDirectory(exportPath);
			//		}

			//		var diskFileName = Path.Combine(exportPath, file.FileName);
			//		using (var fileStream = new FileStream(diskFileName, FileMode.Create))
			//		{
			//			await file.CopyToAsync(fileStream);
			//		}
			//	}
			//}
		}
	}
}
