using HouseDB.DomoticzExporter.SettingModels;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseDB.DomoticzExporter.Exporters
{
	public class ExportValuesForCaching
    {
		private readonly HouseDBSettings _houseDBSettings;
		private DomoticzSettings _domoticzSettings;

        public ExportValuesForCaching(
			DomoticzSettings domoticzSettings,
			HouseDBSettings houseDBSettings)
        {
			_houseDBSettings = houseDBSettings;
			_domoticzSettings = domoticzSettings;
        }

        public async Task DoExport()
        {
            Log.Information("Starting ExportValuesForCaching.DoExport()");

            using (var client = new HttpClient())
            {
                var api = new swaggerClient(_houseDBSettings.ApiBaseUrl, client);
                await api.InsertDomoticzDeviceValuesForCachingAsync(new InsertDomoticzDeviceValuesForCachingRequest
                {
                    DomoticzDeviceValuesForCaching = await GetP1Values(client)
				});
            }
        }

		private async Task<DomoticzDeviceValuesForCaching> GetP1Values(HttpClient client)
		{
			var domoticzData = await GetDomoticzResponse(_domoticzSettings.WattIdx, client);
			var watt = GetDoubleResultValue(domoticzData, "Usage", " Watt");
			var kwh = GetDoubleResultValue(domoticzData, "CounterToday", " kWh");
			var lastUpdate = GetDateTimeResultValue(domoticzData, "LastUpdate");

			var domoticzDeviceValuesForCaching = new DomoticzDeviceValuesForCaching
			{
				DeviceID = 10, // PowerImport1 device
				CurrentWattValue = watt,
				TodayKwhUsage = kwh,
				LastUpdate = lastUpdate
			};

			return domoticzDeviceValuesForCaching;
		}

		//private async Task<DomoticzDeviceValuesForCaching> GetDataValues(Device device, HttpClient client)
		//{
		//	double? watt = 0;
		//	double? kwh = 0;
		//	DateTime? lastUpdate = null;

		//	if (device.DomoticzWattIdx.HasValue && device.DomoticzWattIdx.Value != 0)
		//	{
		//		var domoticzData = await GetDomoticzResponse(device.DomoticzWattIdx.Value, client);
		//		watt = GetDoubleResultValue(domoticzData, "Data", " Watt");
		//		lastUpdate = GetDateTimeResultValue(domoticzData, "LastUpdate");
		//	}

		//	if (device.DomoticzKwhIdx.HasValue && device.DomoticzKwhIdx.Value != 0)
		//	{
		//		var domoticzData = await GetDomoticzResponse(device.DomoticzKwhIdx.Value, client);
		//		kwh = GetDoubleResultValue(domoticzData, "CounterToday", " kWh");
		//		lastUpdate = GetDateTimeResultValue(domoticzData, "LastUpdate");
		//	}

		//	var domoticzDeviceValuesForCaching = new DomoticzDeviceValuesForCaching
		//	{
		//		DeviceID = device.Id,
		//		CurrentWattValue = watt,
		//		TodayKwhUsage = kwh,
		//		LastUpdate = lastUpdate
		//	};

		//	return domoticzDeviceValuesForCaching;
		//}

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
