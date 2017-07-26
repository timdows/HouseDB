using HouseDB.Data;
using HouseDB.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Controllers.VeraExport
{
	[Route("[controller]/[action]")]
	public class MigrateInformationController : HouseDBController
	{
		public MigrateInformationController(DataContext dataContext) : base(dataContext)
		{
		}

		public async Task<JsonResult> Migrate()
		{
			// Define the directory to work from
			var exportPath = Path.Combine(Directory.GetCurrentDirectory(), "exports");
			var extractPath = Path.Combine(exportPath, Path.Combine("extract", "20170609-010458"));

			var devices = _dataContext.Devices
				.Where(a_item => a_item.DataMineChannel != 0 &&
								 a_item.IsForKwhImport)
				.ToList();

			var dataMineConfigFiles = new List<DataMineConfigFile>();
			var kwhDateUsagesList = new List<List<KwhDateUsage>>();

			// Go over every directory in the export path
			foreach (var directoryName in Directory.GetDirectories(extractPath))
			{
				var directory = Path.Combine(extractPath, directoryName);

				// Read the config.json file
				var dataMineConfigFile = await GetDataMineConfigFile(directory);
				dataMineConfigFiles.Add(dataMineConfigFile);

				var device = devices.SingleOrDefault(a_item => a_item.DataMineChannel == dataMineConfigFile.ID);
				if (device == null)
				{
					continue;
				}

				var kwhDateUsages = await GetKwhDateUsages(dataMineConfigFile, device);
				kwhDateUsagesList.Add(kwhDateUsages);
			}

			return Json(new { dataMineConfigFiles, kwhDateUsagesList });
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
