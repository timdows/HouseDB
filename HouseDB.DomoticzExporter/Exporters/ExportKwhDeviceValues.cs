using HouseDB.DomoticzExporter.SettingModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseDB.DomoticzExporter.Exporters
{
    public class ExportKwhDeviceValues
    {
        private readonly DomoticzSettings _domoticzSettings;
        private readonly HouseDBSettings _houseDBSettings;
        private DateTime _lastRunDateTime;

        public ExportKwhDeviceValues(DomoticzSettings domoticzSettings, HouseDBSettings houseDBSettings)
        {
            _domoticzSettings = domoticzSettings;
            _houseDBSettings = houseDBSettings;
            _lastRunDateTime = DateTime.Today.AddDays(-1);
        }

        public async Task DoExport(string additionalRequestUrl = null)
        {
            // Only trigger once per hour
            if (_lastRunDateTime.AddHours(1) > DateTime.Now)
            {
                return;
            }

            _lastRunDateTime = DateTime.Now;
            Log.Information("Starting ExportKwhDeviceValues()");

            using (var client = new HttpClient())
            {
                var api = new swaggerClient(_houseDBSettings.ApiBaseUrl, client);
                var domoticzDevicesForKwhExportResponse = await api.GetDomoticzDevicesForKwhExportAsync(new GetDomoticzDevicesForKwhExportRequest());
                var devices = domoticzDevicesForKwhExportResponse.Devices.ToList();

                foreach (var device in devices)
                {
                    var domoticzDeviceKwhUsages = await GetDomoticzDeviceKwhUsages(device);
                    await api.InsertDomoticzDeviceKwhValuesAsync(new InsertDomoticzDeviceKwhValuesRequest
                    {
                        DeviceId = device.Id,
                        DomoticzDeviceKwhUsages = domoticzDeviceKwhUsages
                    });
                }
            }
        }

        private async Task<List<DomoticzDeviceKwhUsage>> GetDomoticzDeviceKwhUsages(DeviceDTO device)
        {
            using (var client = new HttpClient())
            {
                var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=graph&sensor=counter&idx={device.DomoticzKwhIdx}&range=year";
                var response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<dynamic>(response);
                JArray resultList = data.result;

                // Cast resultList to objects
                var values = resultList.ToObject<List<DomoticzDeviceKwhUsage>>();
                return values;
            }
        }
    }
}
