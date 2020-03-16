using HouseDB.DomoticzExporter.SettingModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseDB.DomoticzExporter.Exporters
{
    public class ExportP1Consumption
    {
        private DomoticzSettings _domoticzSettings;
        private readonly HouseDBSettings _houseDBSettings;

        public ExportP1Consumption(DomoticzSettings domoticzSettings, HouseDBSettings houseDBSettings)
        {
            _domoticzSettings = domoticzSettings;
            _houseDBSettings = houseDBSettings;
        }

        public async Task DoExport()
        {
            Log.Information("Starting ExportP1Consumption()");

            List<DomoticzP1Consumption> domoticzP1Consumptions = null;
            using (var client = new HttpClient())
            {
                // Get the list with values
                var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=graph&sensor=counter&idx={_domoticzSettings.WattIdx}&range=year";
                var response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<dynamic>(response);
                JArray resultList = data.result;

                // Cast resultList to objects
                domoticzP1Consumptions = resultList.ToObject<List<DomoticzP1Consumption>>();

                var api = new swaggerClient(_houseDBSettings.ApiBaseUrl, client);
                await api.InsertP1ConsumptionAsync(new InsertP1ConsumptionRequest
                {
                    DomoticzP1Consumptions = domoticzP1Consumptions
                });
            }

            //using (var client = new HttpClient())
            //{
                
            //}
        }
    }
}
