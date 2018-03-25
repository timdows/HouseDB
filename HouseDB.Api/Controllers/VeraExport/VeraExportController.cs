using HouseDB.Api.Data;
using HouseDB.Api.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers.VeraExport
{
	[Route("[controller]/[action]")]
	public class VeraExportController : HouseDBController
	{
		private readonly ILogger<VeraExportController> _logger;

		public VeraExportController(
			DataContext dataContext,
			ILogger<VeraExportController> logger
			) : base(dataContext)
		{
			_logger = logger;
		}

		[HttpPost]
		public async Task<JsonResult> Test([FromQuery] string localPath)
		{
			await ImportKwhDeviceValues(localPath);

			return Json(true);
		}

		[HttpPost]
		public async Task<JsonResult> Upload(ICollection<IFormFile> files)
		{
			_logger.LogWarning("VeraExportController files count {0}", files.Count);

			foreach (var file in files)
			{
				_logger.LogWarning(file.FileName);
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

					// Extract zipfile
					var extractPath = Path.Combine(exportPath, Path.Combine("extract", dateTime.ToString("yyyyMMdd-HHmmss")));
					if (!Directory.Exists(exportPath))
					{
						Directory.CreateDirectory(exportPath);
					}

					ZipFile.ExtractToDirectory(diskFileName, extractPath);

					// Import values
					await ImportKwhDeviceValues(extractPath);
				}
			}

			return Json(true);
		}

		[HttpGet]
		public JsonResult GetExportFileStats()
		{
			var last10Stats = _dataContext.ExportFiles
				.OrderByDescending(a_item => a_item.DateAdded)
				.Take(10)
				.ToList();

			return Json(last10Stats);
		}

		private async Task ImportKwhDeviceValues(string extractPath)
		{
			_logger.LogWarning("ImportKwhDeviceValues extractPath {0}", extractPath);

			var devices = _dataContext.Devices
				.Where(a_item => a_item.IsForKwhImport)
				.ToList();

			foreach (var device in devices)
			{
				var rawPath = Path.Combine(extractPath, Path.Combine(device.DataMineChannel.ToString(), "raw"));
				var rawFiles = Directory.GetFiles(rawPath, "*.txt");

				_logger.LogWarning($"{rawFiles.Length} files for device {device.Name}");

				// Get raw information and parse to list
				var rawKwhDeviceValues = new List<KwhDeviceValue>();
				foreach (var rawFile in rawFiles)
				{
					string[] lines;
					using (var reader = System.IO.File.OpenText(rawFile))
					{
						var fileText = await reader.ReadToEndAsync();
						lines = fileText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
					}

					foreach (var line in lines)
					{
						var kwhDeviceValue = KwhDeviceValue.Create(line, device);
						if (kwhDeviceValue != null)
						{
							rawKwhDeviceValues.Add(kwhDeviceValue);
						}
					}
				}

				// Get exising values from database
				var existingValues = _dataContext.KwhDeviceValues
					.Where(a_item => a_item.DeviceID == device.ID)
					.ToList();

				// Compare lists
				var newKwhDeviceValues = rawKwhDeviceValues
					.Except(existingValues)
					.ToList();

				if (!newKwhDeviceValues.Any())
				{
					_logger.LogWarning("Nothing to import for {0}", device.Name);
					continue;
				}

				existingValues = null;

				int perIteration = 1000;
				while (newKwhDeviceValues.Count >= perIteration)
				{
					var range = newKwhDeviceValues.GetRange(0, perIteration);
					newKwhDeviceValues.RemoveRange(0, perIteration);

					_logger.LogWarning("Lines left for device {0}: {1}", device.Name, newKwhDeviceValues.Count);

					await _dataContext.KwhDeviceValues.AddRangeAsync(range);
					await _dataContext.SaveChangesAsync();
				}

				_logger.LogWarning("Lines left for device {0}: {1}", device.Name, newKwhDeviceValues.Count);

				await _dataContext.KwhDeviceValues.AddRangeAsync(newKwhDeviceValues);
				await _dataContext.SaveChangesAsync();
			}
		}
	}
}
