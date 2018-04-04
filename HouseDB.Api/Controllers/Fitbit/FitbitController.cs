using HouseDB.Api.Data;
using HouseDB.Api.Data.Fitbit;
using HouseDB.Api.Data.Models;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers.Fitbit
{
	[Route("[controller]/[action]")]
	public class FitbitController : HouseDBController
	{
		private readonly FitbitSettings _fitbitSettings;

		public FitbitController(DataContext dataContext,
			IOptions<FitbitSettings> fitbitSettings) : base(dataContext)
		{
			_fitbitSettings = fitbitSettings.Value;
		}

		[HttpPost]
		public async Task InsertCallback([FromBody] InsertCallbackClientModel insertCallbackClientModel)
		{
			var fitbitClientDetail = _dataContext.FitbitClientDetails
				.Single(a_item => a_item.ClientId == insertCallbackClientModel.ClientId);

			var fitbitAuthCode = new FitbitAuthCode
			{
				FitbitClientDetail = fitbitClientDetail,
				AuthCode = insertCallbackClientModel.AuthCode,
				DateTimeAdded = DateTime.Now
			};

			_dataContext.FitbitAuthCodes.Add(fitbitAuthCode);
			
			var accessToken = await ExchangeAuthCodeForAccessTokenAsync(fitbitAuthCode);

			var fitbitAccessToken = new FitbitAccessToken
			{
				FitbitAuthCode = fitbitAuthCode,
				AccessToken = accessToken,
				DateTimeAdded = DateTime.Now
			};

			_dataContext.FitbitAccessTokens.Add(fitbitAccessToken);
			await _dataContext.SaveChangesAsync();
		}

		private async Task<string> ExchangeAuthCodeForAccessTokenAsync(FitbitAuthCode fitbitAuthCode)
		{
			using (var httpClient = new HttpClient())
			{
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("grant_type", "authorization_code"),
					new KeyValuePair<string, string>("client_id", fitbitAuthCode.FitbitClientDetail.ClientId),
					new KeyValuePair<string, string>("code", fitbitAuthCode.AuthCode),
					new KeyValuePair<string, string>("redirect_uri", _fitbitSettings.CallbackUrl)
				});

				var clientIdConcatSecret = $"{fitbitAuthCode.FitbitClientDetail.ClientId}:{fitbitAuthCode.FitbitClientDetail.ClientSecret}";
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(clientIdConcatSecret));

				HttpResponseMessage response = await httpClient.PostAsync(_fitbitSettings.AccessAndRefreshUrl, content);
				string responseString = await response.Content.ReadAsStringAsync();

				JObject responseObject = JObject.Parse(responseString);

				// Note: if user cancels the auth process Fitbit returns a 200 response, but the JSON payload is way different.
				var error = responseObject["error"];
				if (error != null)
				{
					// TODO: Actually should probably raise an exception here maybe?
					return null;
				}

				return responseObject["access_token"].ToString();
			}



			//OAuth2AccessToken accessToken = new OAuth2AccessToken();

			//var temp_access_token = responseObject["access_token"];
			//if (temp_access_token != null) accessToken.Token = temp_access_token.ToString();

			//var temp_expires_in = responseObject["expires_in"];
			//if (temp_expires_in != null) accessToken.ExpiresIn = Convert.ToInt32(temp_expires_in.ToString());

			//var temp_token_type = responseObject["token_type"];
			//if (temp_token_type != null) accessToken.TokenType = temp_token_type.ToString();

			//var temp_refresh_token = responseObject["refresh_token"];
			//if (temp_refresh_token != null) accessToken.RefreshToken = temp_refresh_token.ToString();

			//return accessToken;
		}

		private string Base64Encode(string v)
		{
			var byt = System.Text.Encoding.UTF8.GetBytes(v);
			return Convert.ToBase64String(byt);
		}

		private async Task<JObject> GetActivity(string token)
		{
			using (var client = new HttpClient())
			{
				//https://api.fitbit.com/1/user/-/activities/steps/date/today/1y.json
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await client.GetStringAsync("https://api.fitbit.com/1/user/-/activities/steps/date/today/1y.json");
				JObject responseObject = JObject.Parse(response);

				return responseObject;
			}
		}
	}
}
