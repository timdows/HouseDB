using HouseDB.Core;
using HouseDB.Core.Settings;
using HouseDB.Services.Api;
using HouseDB.Services.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Exporter.Exporters
{
	public class ExportMotionDetection
    {
		private HouseDBSettings _houseDBSettings;
		private DomoticzSettings _domoticzSettings;
		private JwtTokenManager _jwtTokenManager;
		private IList<Device> _devices;

		public ExportMotionDetection(
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
				_devices = api.DeviceGetAllMotionDetectionDevicesGet();
			}
		}

		public async Task DoExport()
		{
			Log.Information("Starting ExportMotionDetection - DoExport");

			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				foreach (var device in _devices)
				{
					var clientModel = await GetMotionDetectionClientModel(device, true);
					if (clientModel == null)
					{
						continue;
					}

					var token = _jwtTokenManager.GetToken(_houseDBSettings).GetAwaiter().GetResult();
					api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
					await api.ExporterInsertMotionDetectionValuesPostAsync(clientModel);
				}
			}
		}

		private async Task<DomoticzMotionDetectionClientModel> GetMotionDetectionClientModel(Device device, bool onlyExportOneHour)
		{
			using (var client = new HttpClient())
			{
				var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/json.htm?type=lightlog&idx={device.DomoticzMotionDetectionIdx}";
				var response = await client.GetStringAsync(url);
				var data = JsonConvert.DeserializeObject<dynamic>(response);
				JArray resultList = data.result;

				if (resultList == null)
				{
					return null;
				}

				// Cast resultList to objects
				var values = resultList.ToObject<List<DomoticzMotionDetection>>();

				if (onlyExportOneHour)
				{
					var timeLimit = DateTime.Today.AddHours(-1);
					values = values
						.Where(a_item => a_item.Date.Value.Date >= timeLimit)
						.ToList();
				}

				var clientModel = new DomoticzMotionDetectionClientModel
				{
					Device = device,
					MotionDetections = values
				};

				return clientModel;
			}
		}
	}
}
