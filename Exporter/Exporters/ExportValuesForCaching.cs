using Exporter.HouseDBService;
using Exporter.HouseDBService.Models;
using HouseDBCore;
using HouseDBCore.Settings;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Exporter.Exporters
{
	public class ExportValuesForCaching
    {
		private HouseDBSettings _houseDBSettings;
		private JwtTokenManager _jwtTokenManager;
		private DomoticzSettings _domoticzSettings;
		private DateTime _lastExportDateTime;
		private IList<Device> _devices;

		public ExportValuesForCaching(
			HouseDBSettings houseDBSettings,
			JwtTokenManager jwtTokenManager,
			DomoticzSettings domoticzSettings)
		{
			_houseDBSettings = houseDBSettings;
			_domoticzSettings = domoticzSettings;
			_jwtTokenManager = jwtTokenManager;
			_lastExportDateTime = DateTime.Today.AddDays(-1);

			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = _jwtTokenManager.GetToken(_houseDBSettings).GetAwaiter().GetResult();
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				_devices = api.DeviceGetAllDevicesForCachingValuesGet();
			}
		}

		public async Task DoExport()
		{
			Log.Information("Starting ExportValuesForCaching.DoExport()");

			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var clientModel = new DomoticzValuesForCachingClientModel
				{
					DateTime = DateTime.Now,
					DomoticzValuesForCachingValues = new List<DomoticzValuesForCachingValue>()
				};

				using (var client = new HttpClient())
				{
					// Get the values for P1 (smart home meter)
					clientModel.P1Values = await GetP1Values(client);

					// Get the values to cache for every device
					foreach (var device in _devices)
					{
						var value = await GetDataValues(device, client);
						clientModel.DomoticzValuesForCachingValues.Add(value);
					}
				}

				var token = _jwtTokenManager.GetToken(_houseDBSettings).GetAwaiter().GetResult();
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				await api.ExporterInsertValuesForCachingPostAsync(clientModel);
			}
		}

		private async Task<DomoticzValuesForCachingValue> GetP1Values(HttpClient client)
		{
			var watt = await GetDataResultValue(_domoticzSettings.WattIdx.Value, "Usage", " Watt", client);
			var kwh = await GetDataResultValue(_domoticzSettings.WattIdx.Value, "CounterToday", " kWh", client);

			var domoticzValuesForCachingValue = new DomoticzValuesForCachingValue
			{
				DeviceID = 10, // PowerImport1 device
				CurrentWattValue = watt,
				TodayKwhUsage = kwh
			};

			return domoticzValuesForCachingValue;
		}

		private async Task<DomoticzValuesForCachingValue> GetDataValues(Device device, HttpClient client)
		{
			double? watt = 0;
			double? kwh = 0;

			if (device.DomoticzWattIdx.HasValue && device.DomoticzWattIdx.Value != 0)
			{
				watt = await GetDataResultValue(device.DomoticzWattIdx.Value, "Data", " Watt", client);
			}
			
			if (device.DomoticzKwhIdx.HasValue && device.DomoticzKwhIdx.Value != 0)
			{
				kwh = await GetDataResultValue(device.DomoticzKwhIdx.Value, "CounterToday", " kWh", client);
			}

			var domoticzValuesForCachingValue = new DomoticzValuesForCachingValue
			{
				DeviceID = device.Id,
				CurrentWattValue = watt,
				TodayKwhUsage = kwh
			};

			return domoticzValuesForCachingValue;
		}

		private async Task<double> GetDataResultValue(int idx, string nameInObject, string replaceString, HttpClient client)
		{
			var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=devices&rid={idx}";
			var response = await client.GetStringAsync(url);
			var data = JsonConvert.DeserializeObject<dynamic>(response);

			string valueString = data.result[0][nameInObject].ToString().Replace(replaceString, string.Empty);
			double value = double.Parse(valueString);

			return value;
		}
	}
}