using HouseDB.Api.Data;
using HouseDB.Api.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers.VeraExport
{
	[Route("[controller]/[action]")]
	public class MigrateInformationController : HouseDBController
	{
		public MigrateInformationController(DataContext dataContext) : base(dataContext)
		{
		}

		[HttpGet]
		public async Task<JsonResult> Migrate()
		{
			// Define the directory to work from
			var exportPath = Path.Combine(Directory.GetCurrentDirectory(), "exports");
			var extractPath = Path.Combine(exportPath, Path.Combine("extract", "20170327-222355"));

			var devices = _dataContext.Devices
				.Where(a_item => a_item.DataMineChannel != 0 &&
								 a_item.IsForKwhImport)
				.ToList();

			var dataMineConfigFiles = new List<DataMineConfigFile>();

			// Go over every directory in the export path and use async tasks for database savings
			var saveChangesTasks = new List<Task>();
			foreach (var directoryName in Directory.GetDirectories(extractPath))
			{
				var directory = Path.Combine(extractPath, directoryName);

				// Read the config.json file
				var dataMineConfigFile = await GetDataMineConfigFile(directory);
				dataMineConfigFiles.Add(dataMineConfigFile);

				// Check if we know the dataMine configuration
				var device = devices.SingleOrDefault(a_item => a_item.DataMineChannel == dataMineConfigFile.ID);
				if (device == null)
				{
					continue;
				}

				var kwhDateUsages = await GetKwhDateUsages(dataMineConfigFile, device);

				// Check if values are already in the database
				var oldestDate = kwhDateUsages.Min(a_item => a_item.Date);
				var newestDate = kwhDateUsages.Max(a_item => a_item.Date);
				var existingDates = _dataContext.KwhDateUsages
					.Where(a_item => a_item.DeviceID == device.ID &&
									 a_item.Date >= oldestDate &&
									 a_item.Date <= newestDate)
					.Select(a_item => a_item.Date)
					.ToList();

				foreach(var kwhDateUsage in kwhDateUsages)
				{
					if (!existingDates.Contains(kwhDateUsage.Date))
					{
						_dataContext.KwhDateUsages.Add(kwhDateUsage);
					}
				}

				saveChangesTasks.Add(_dataContext.SaveChangesAsync());
			}

			await Task.WhenAll(saveChangesTasks);

			return Json(new { dataMineConfigFiles });
		}

		private async Task<DataMineConfigFile> GetDataMineConfigFile(string directory)
		{
			var configFile = Path.Combine(directory, "config.json");

			using (var reader = System.IO.File.OpenText(configFile))
			{
				var configText = await reader.ReadToEndAsync();
				var dataMineConfigFile = JsonConvert.DeserializeObject<DataMineConfigFile>(configText);

				dataMineConfigFile.Directory = directory;

				return dataMineConfigFile;
			}
		}

		private async Task<List<KwhDateUsage>> GetKwhDateUsages(DataMineConfigFile dataMineConfigFile, Data.Models.Device device)
		{
			var rawPath = Path.Combine(dataMineConfigFile.Directory, "raw");
			var rawFiles = Directory.GetFiles(rawPath, "*.txt");

			Log.Debug($"{rawFiles.Length} files for device {dataMineConfigFile.Name}");

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

			// Get total usages per day
			var kwhDateUsages = new List<KwhDateUsage>();
			var oldestDate = rawKwhDeviceValues.Min(a_item => a_item.DateTime);
			var newestDate = rawKwhDeviceValues.Max(a_item => a_item.DateTime);

			for (var date = oldestDate; date <= newestDate; date = date.AddDays(1))
			{
				var dayUsages = rawKwhDeviceValues
					.Where(a_item => a_item.DateTime.Date == date.Date);

				if (!dayUsages.Any())
				{
					continue;
				}

				var minUsage = dayUsages.Min(a_item => a_item.Value);
				var maxUsage = dayUsages.Max(a_item => a_item.Value);
				var dayUsage = maxUsage - minUsage;

				var kwhDateUsage = new KwhDateUsage
				{
					Date = date,
					DeviceID = device.ID,
					Usage = dayUsage
				};

				kwhDateUsages.Add(kwhDateUsage);
			}

			return kwhDateUsages;
		}
	}

	public class DataMineConfigFile
	{
		public string Service { get; set; }
		public int Device { get; set; }
		public int ID { get; set; }
		public string Name { get; set; }
		public string Variable { get; set; }
		public int DataType { get; set; }
		public string Directory { get; set; }
	}
}
