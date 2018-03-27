﻿using HouseDB.Core;
using HouseDB.Core.Settings;
using HouseDB.Services.Api;
using HouseDB.Services.Api.Models;
using Serilog;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Exporter.Exporters
{
	public class ExportDatabase
    {
		private HouseDBSettings _houseDBSettings;
		private DomoticzSettings _domoticzSettings;
		private JwtTokenManager _jwtTokenManager;
		private DateTime _lastExportDatabase;

		public ExportDatabase(
			HouseDBSettings houseDBSettings,
			JwtTokenManager jwtTokenManager,
			DomoticzSettings domoticzSettings)
		{
			_houseDBSettings = houseDBSettings;
			_domoticzSettings = domoticzSettings;
			_jwtTokenManager = jwtTokenManager;
			_lastExportDatabase = DateTime.Today.AddDays(-1);
		}

		public async Task DoExport()
		{
			// Check if we should export
			if (DateTime.Now.Hour != 0 || DateTime.Now.Minute != 0 || (DateTime.Now - _lastExportDatabase).TotalHours < 23)
			{
				return;
			}

			_lastExportDatabase = DateTime.Now;
			Log.Information("Starting ExportDatabase");
			byte[] byteArray;

			// Get the database
			using (var client = new HttpClient())
			{
				client.Timeout = TimeSpan.FromMinutes(1);

				var url = $"http://{_domoticzSettings.Host}:{_domoticzSettings.Port}/backupdatabase.php";
				using (var response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
				{
					response.EnsureSuccessStatusCode();
					byteArray = await response.Content.ReadAsByteArrayAsync();
				}
			}

			// Post the database to the HouseDBAPI
			using (var api = new HouseDBAPI(new Uri(_houseDBSettings.ApiUrl)))
			{
				var token = _jwtTokenManager.GetToken(_houseDBSettings).GetAwaiter().GetResult();
				api.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				await api.ExporterUploadDatabasePostAsync(new DomoticzPostDatabaseFile
				{
					ByteArray = byteArray,
					DateTime = DateTime.Now,
					FileName = "domoticz.db"
				});
			}
		}
	}
}