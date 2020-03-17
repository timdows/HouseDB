using HouseDB.DomoticzExporter.SettingModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseDB.DomoticzExporter.Exporters
{
    public class ExportP1Consumption
    {
        private DomoticzSettings _domoticzSettings;
        private readonly HouseDBSettings _houseDBSettings;
        private DateTime _lastRunDateTime;

        public ExportP1Consumption(DomoticzSettings domoticzSettings, HouseDBSettings houseDBSettings)
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
            Log.Information("Starting ExportP1Consumption()");

            using (var client = new HttpClient())
            {
                // Get the list with values
                var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=graph&sensor=counter&idx={_domoticzSettings.WattIdx}&range=year{additionalRequestUrl}";
                var response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<dynamic>(response);
                JArray resultList = data.result;

                // Cast resultList to objects
                var domoticzP1Consumptions = resultList.ToObject<List<DomoticzP1Consumption>>();

                var api = new swaggerClient(_houseDBSettings.ApiBaseUrl, client);
                await api.InsertP1ConsumptionAsync(new InsertP1ConsumptionRequest
                {
                    DomoticzP1Consumptions = domoticzP1Consumptions
                });
            }
        }

        public async Task DoExportMultipleYears()
        {
            for (int i = 2010; i <= DateTime.Today.Year; i++)
            {
                await DoExport($"&actyear={i}");
            }
        }
    }
}
