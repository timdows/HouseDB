using HouseDB.Core;
using HouseDB.Core.Settings;
using HouseDB.Exporter.Exporters;
using HouseDB.Services.Api.Models;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HouseDB.Exporter
{
	public class Application
    {
		private HouseDBSettings _houseDBSettings;
		private DomoticzSettings _domoticzSettings;
		private JwtTokenManager _jwtTokenManager;

		public Application(
			HouseDBSettings houseDBSettings,
			JwtTokenManager jwtTokenManager,
			DomoticzSettings domoticzSettings)
		{
			_houseDBSettings = houseDBSettings;
			_jwtTokenManager = jwtTokenManager;
			_domoticzSettings = domoticzSettings;
		}

		public async Task Run()
		{
			Log.Information("Starting Application.Run()");

			var exportP1Consumption = new ExportP1Consumption(_houseDBSettings, _jwtTokenManager, _domoticzSettings);
			var exportKwhDeviceValues = new ExportKwhDeviceValues(_houseDBSettings, _jwtTokenManager, _domoticzSettings);
			var exportValuesForCaching = new ExportValuesForCaching(_houseDBSettings, _jwtTokenManager, _domoticzSettings);
			var exportDatabase = new ExportDatabase(_houseDBSettings, _jwtTokenManager, _domoticzSettings);
			var exportMotionDetection = new ExportMotionDetection(_houseDBSettings, _jwtTokenManager, _domoticzSettings);

			while (true)
			{
				try
				{
					await Task.WhenAll(
						exportP1Consumption.DoExport(),
						exportKwhDeviceValues.DoExport(),
						exportValuesForCaching.DoExport(),
						exportDatabase.DoExport(),
						exportMotionDetection.DoExport(),
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
