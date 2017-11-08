using Exporter.HouseDBService;
using Exporter.HouseDBService.Models;
using HouseDBCore;
using HouseDBCore.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Exporter.Exporters
{
	/// <summary>
	/// Gets all (range of a year) the kWh values for selected devices and exports them to get saved
	/// The export contains the complete energy usage per day per device
	/// </summary>
	public class ExportKwhDeviceValues
    {
		private HouseDBSettings _houseDBSettings;
		private JwtTokenManager _jwtTokenManager;
		private DomoticzSettings _domoticzSettings;
		private DateTime _lastExportDateTime;
		private IList<Device> _devices;

		public ExportKwhDeviceValues(
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
				_devices = api.DeviceGetAllKwhExportDevicesGet();
			}
		}

		public async Task DoExport()
		{
			if (DateTime.Now.Hour == 0 && (DateTime.Now - _lastExportDateTime).Hours > 23)
			{
				_lastExportDateTime = DateTime.Now;
				Log.Debug("Starting ExportKwhDeviceValues - DoExport");

				using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
				{
					var token = _jwtTokenManager.GetToken(_houseDBSettings).GetAwaiter().GetResult();
					api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

					foreach (var device in _devices)
					{
						var clientModel = await GetDomoticzKwhValuesClientModel(device);
						await api.ExporterInsertDomoticzKwhValuesPostAsync(clientModel);
					}
				}
			}
		}

		private async Task<DomoticzKwhValuesClientModel> GetDomoticzKwhValuesClientModel(Device device)
		{
			using (var client = new HttpClient())
			{
				var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=graph&sensor=counter&idx={device.DomoticzKwhIdx}&range=year";
				var response = await client.GetStringAsync(url);
				var data = JsonConvert.DeserializeObject<dynamic>(response);
				JArray resultList = data.result;

				// Cast resultList to objects
				var values = resultList.ToObject<List<DomoticzKwhUsage>>();

				var clientModel = new DomoticzKwhValuesClientModel
				{
					Device = device,
					DomoticzKwhUsages = values
				};

				return clientModel;
			}
		}
	}
}
