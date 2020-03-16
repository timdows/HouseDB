using HouseDB.Core.DomoticzModels;
using HouseDB.Core.SettingModels;
using HouseDB.Core.UseCases.P1Consumption;
using MediatR;
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
        private readonly IMediator _mediator;

        public ExportP1Consumption(DomoticzSettings domoticzSettings, IMediator mediator)
        {
            _domoticzSettings = domoticzSettings;
            _mediator = mediator;
        }

        public async Task DoExport()
        {
            Log.Information("Starting ExportP1Consumption()");

            using (var client = new HttpClient())
            {
                // Get the list with values
                var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=graph&sensor=counter&idx={_domoticzSettings.WattIdx}&range=year";
                var response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<dynamic>(response);
                JArray resultList = data.result;

                // Cast resultList to objects
                var domoticzP1Consumptions = resultList.ToObject<List<DomoticzP1Consumption>>();

                var insertP1ConsumptionRequest = new InsertP1ConsumptionRequest
                {
                    DomoticzP1Consumptions = domoticzP1Consumptions
                };
                var insertP1ConsumptionResponse = await _mediator.Send(insertP1ConsumptionRequest);

                //// Post it away
                //using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
                //{
                //    var token = _jwtTokenManager.GetToken(_houseDBSettings).GetAwaiter().GetResult();
                //    api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //    await api.ExporterInsertDomoticzP1ConsumptionPostAsync(values);
                //}
            }
        }
    }
}
