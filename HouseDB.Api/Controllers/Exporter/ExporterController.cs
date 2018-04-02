using HouseDB.Api.Data;
using HouseDB.Api.Data.Exporter;
using HouseDB.Api.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers.Exporter
{
	[Route("[controller]/[action]")]
	public class ExporterController : HouseDBController
	{
		private readonly IMemoryCache _memoryCache;
		private List<DateTime> _inDatabaseP1Consumptinos;

		public ExporterController(DataContext dataContext, IMemoryCache memoryCache) : base(dataContext)
		{
			_memoryCache = memoryCache;
			_inDatabaseP1Consumptinos = _dataContext.P1Consumptions
				.Select(a_item => a_item.Date)
				.ToList();
		}

		[HttpPost]
		public void InsertDomoticzP1Consumption([FromBody] List<DomoticzP1Consumption> domoticzP1Consumptions)
		{
			// Save in memory cache to be used by sevensegment
			_memoryCache.Set(nameof(List<DomoticzP1Consumption>), domoticzP1Consumptions);

			// Save new entries in database
			var newData = domoticzP1Consumptions
				.Where(a_item => !_inDatabaseP1Consumptinos.Contains(a_item.Date))
				.ToList();

			if (newData.Any())
			{
				foreach (var item in newData)
				{
					_dataContext.P1Consumptions.Add(new P1Consumption
					{
						Date = item.Date,
						DayUsage = item.DayUsage
					});
				}

				_dataContext.SaveChangesAsync();
			}
		}

		[HttpPost]
		public async Task InsertDomoticzKwhValues([FromBody] DomoticzKwhValuesClientModel clientModel)
		{
			// Check if there is something to import
			if (clientModel.Device?.ID == null || !clientModel.DomoticzKwhUsages.Any())
			{
				return;
			}

			var minDate = clientModel.DomoticzKwhUsages.Min(b_item => b_item.Date);
			var maxDate = clientModel.DomoticzKwhUsages.Max(b_item => b_item.Date);

			// Get the existing values from this device between the clientModel dates
			var existingDates = _dataContext.KwhDateUsages
				.Where(a_item => a_item.DeviceID == clientModel.Device.ID &&
								 a_item.Date >= minDate &&
								 a_item.Date <= maxDate)
				.Select(a_item => a_item.Date)
				.ToList();

			foreach (var domoticzKwhUsage in clientModel.DomoticzKwhUsages)
			{
				// Skip value if it is already in the database or if it is today
				if (existingDates.Any(a_item => a_item == domoticzKwhUsage.Date) || domoticzKwhUsage.Date == DateTime.Today)
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
		public async Task InsertMotionDetectionValues([FromBody] DomoticzMotionDetectionClientModel clientModel)
		{
			// Check if there is something to import
			if (clientModel.Device?.ID == null || !clientModel.MotionDetections.Any())
			{
				return;
			}

			var minDate = clientModel.MotionDetections.Min(b_item => b_item.Date);
			var maxDate = clientModel.MotionDetections.Max(b_item => b_item.Date);

			// Get the existing values from this device between the clientModel dates
			var motionDetectionDates = _dataContext.MotionDetections
				.Where(a_item => a_item.DeviceID == clientModel.Device.ID &&
								 a_item.DateTimeDetection >= minDate &&
								 a_item.DateTimeDetection <= maxDate)
				.Select(a_item => a_item.DateTimeDetection)
				.ToList();

			var saveTasks = new List<Task>();
			var count = 0;
			foreach (var motionDetection in clientModel.MotionDetections)
			{
				// Skip value if it is already in the database
				if (motionDetectionDates.Any(a_item => a_item == motionDetection.Date))
				{
					continue;
				}

				_dataContext.MotionDetections.Add(new MotionDetection
				{
					DeviceID = clientModel.Device.ID,
					DateTimeDetection = motionDetection.Date,
					Status = motionDetection.Status.Equals("On", StringComparison.CurrentCultureIgnoreCase)
				});

				if (++count == 500)
				{
					saveTasks.Add(_dataContext.SaveChangesAsync());
					count = 0;
				}
			}

			saveTasks.Add(_dataContext.SaveChangesAsync());
			await Task.WhenAll(saveTasks);
		}

		[HttpPost]
		public async Task UploadDatabase([FromBody] DomoticzPostDatabaseFile domoticzPostDatabaseFile)
		{
			if (domoticzPostDatabaseFile.ByteArray.Length == 0)
			{
				return;
			}

			// Save the information to database
			var dateTime = DateTime.Now;
			var fileName = $"{dateTime.ToString("yyyyMMdd-HHmmss")}-{domoticzPostDatabaseFile.FileName}";
			var exportFile = new ExportFile
			{
				Length = domoticzPostDatabaseFile.ByteArray.Length,
				DateAdded = dateTime,
				FileName = fileName
			};

			await _dataContext.AddAsync(exportFile);
			await _dataContext.SaveChangesAsync();

			// Save file to disk
			var exportPath = Path.Combine(Directory.GetCurrentDirectory(), "exports");
			if (!Directory.Exists(exportPath))
			{
				Directory.CreateDirectory(exportPath);
			}

			var diskFileName = Path.Combine(exportPath, fileName);
			System.IO.File.WriteAllBytes(diskFileName, domoticzPostDatabaseFile.ByteArray);
		}
	}
}
