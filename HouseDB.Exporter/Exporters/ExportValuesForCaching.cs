using HouseDB.Exporter.HouseDBService;
using HouseDB.Exporter.HouseDBService.Models;
using HouseDB.Core;
using HouseDB.Core.Settings;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Exporter.Exporters
{
	public class ExportValuesForCaching
    {
		private HouseDBSettings _houseDBSettings;
		private JwtTokenManager _jwtTokenManager;
		private DomoticzSettings _domoticzSettings;
		private IList<Device> _devices;

		public ExportValuesForCaching(
			HouseDBSettings houseDBSettings,
			JwtTokenManager jwtTokenManager,
			DomoticzSettings domoticzSettings)
		{
			_houseDBSettings = houseDBSettings;
			_domoticzSettings = domoticzSettings;
			_jwtTokenManager = jwtTokenManager;

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
			var domoticzData = await GetDomoticzResponse(_domoticzSettings.WattIdx.Value, client);
			var watt = GetDoubleResultValue(domoticzData, "Usage", " Watt");
			var kwh = GetDoubleResultValue(domoticzData, "CounterToday", " kWh");
			var lastUpdate = GetDateTimeResultValue(domoticzData, "LastUpdate");

			var domoticzValuesForCachingValue = new DomoticzValuesForCachingValue
			{
				DeviceID = 10, // PowerImport1 device
				CurrentWattValue = watt,
				TodayKwhUsage = kwh,
				LastUpdate = lastUpdate
			};

			return domoticzValuesForCachingValue;
		}

		private async Task<DomoticzValuesForCachingValue> GetDataValues(Device device, HttpClient client)
		{
			double? watt = 0;
			double? kwh = 0;
			DateTime? lastUpdate = null;

			if (device.DomoticzWattIdx.HasValue && device.DomoticzWattIdx.Value != 0)
			{
				var domoticzData = await GetDomoticzResponse(device.DomoticzWattIdx.Value, client);
				watt = GetDoubleResultValue(domoticzData, "Data", " Watt");
				lastUpdate = GetDateTimeResultValue(domoticzData, "LastUpdate");
			}
			
			if (device.DomoticzKwhIdx.HasValue && device.DomoticzKwhIdx.Value != 0)
			{
				var domoticzData = await GetDomoticzResponse(device.DomoticzKwhIdx.Value, client);
				kwh = GetDoubleResultValue(domoticzData, "CounterToday", " kWh");
				lastUpdate = GetDateTimeResultValue(domoticzData, "LastUpdate");
			}

			var domoticzValuesForCachingValue = new DomoticzValuesForCachingValue
			{
				DeviceID = device.Id,
				CurrentWattValue = watt,
				TodayKwhUsage = kwh,
				LastUpdate = lastUpdate
			};

			return domoticzValuesForCachingValue;
		}

		private double GetDoubleResultValue(dynamic data, string nameInObject, string replaceString)
		{
			string valueString = data.result[0][nameInObject].ToString().Replace(replaceString, string.Empty);
			return double.Parse(valueString, CultureInfo.InvariantCulture);
		}

		private DateTime? GetDateTimeResultValue(dynamic data, string nameInObject)
		{
			string valueString = data.result[0][nameInObject].ToString();
			return DateTime.TryParse(valueString, out var dateTime)
				? (DateTime?)dateTime
				: null;
		}

		private async Task<dynamic> GetDomoticzResponse(int idx, HttpClient client)
		{
			var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=devices&rid={idx}";
			var response = await client.GetStringAsync(url);
			return JsonConvert.DeserializeObject<dynamic>(response);
		}
	}
}