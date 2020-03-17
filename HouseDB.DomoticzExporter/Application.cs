using HouseDB.DomoticzExporter.Exporters;
using HouseDB.DomoticzExporter.SettingModels;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HouseDB.DomoticzExporter
{
    public class Application
    {
        private readonly DomoticzSettings _domoticzSettings;
        private readonly HouseDBSettings _houseDBSettings;

        public Application(
            IOptions<DomoticzSettings> domoticzSettings,
            IOptions<HouseDBSettings> houseDBSettings)
        {
            _domoticzSettings = domoticzSettings.Value;
            _houseDBSettings = houseDBSettings.Value;
        }

        public async Task Run()
        {
            Log.Information("Starting Application.Run()");

            var exportP1Consumption = new ExportP1Consumption(_domoticzSettings, _houseDBSettings);
            var exportValuesForCaching = new ExportValuesForCaching(_domoticzSettings, _houseDBSettings);
            var exportKwhDeviceValues = new ExportKwhDeviceValues(_domoticzSettings, _houseDBSettings);

            //await exportP1Consumption.DoExportMultipleYears();
            //await exportKwhDeviceValues.DoExportMultipleYears();

            while (true)
            {
                try
                {
                    await Task.WhenAll(
                        exportP1Consumption.DoExport(),
                        exportValuesForCaching.DoExport(),
                        exportKwhDeviceValues.DoExport(),
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
