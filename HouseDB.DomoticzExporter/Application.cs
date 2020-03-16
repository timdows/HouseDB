using HouseDB.Core.Interfaces;
using HouseDB.Core.SettingModels;
using HouseDB.DomoticzExporter.Exporters;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HouseDB.DomoticzExporter
{
    public class Application
    {
        private readonly DomoticzSettings _domoticzSettings;
        private readonly IMediator _mediator;
        private readonly IP1ConsumptionRepository _p1ConsumptionRepository;

        public Application(
            IOptions<DomoticzSettings> domoticzSettings,
            IMediator mediator,
            IP1ConsumptionRepository p1ConsumptionRepository)
        {
            _domoticzSettings = domoticzSettings.Value;
            _mediator = mediator;
            _p1ConsumptionRepository = p1ConsumptionRepository;
        }

        public async Task Run()
        {
            Log.Information("Starting Application.Run()");

            var exportP1Consumption = new ExportP1Consumption(_domoticzSettings, _mediator);

            while (true)
            {
                try
                {
                    await Task.WhenAll(
                        exportP1Consumption.DoExport(),
                        //exportKwhDeviceValues.DoExport(),
                        //exportValuesForCaching.DoExport(),
                        //exportDatabase.DoExport(),
                        //exportMotionDetection.DoExport(),
                        Task.Delay(5000));
                }
                catch (Exception excep)
                {
                    Log.Fatal(excep.Message);
                }
            }
        }
    }
}
