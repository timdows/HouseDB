using HouseDB.Api.Data;
using HouseDB.Api.Data.Models;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

		[HttpGet]
		public async Task<IActionResult> GetActivity(string clientId)
		{
			var fitbitAccessToken = _dataContext.FitbitAccessTokens
				.Include(a_item => a_item.FitbitAuthCode.FitbitClientDetail)
				.Where(a_item => a_item.FitbitAuthCode.FitbitClientDetail.ClientId == clientId)
				.OrderByDescending(a_item => a_item.DateTimeAdded)
				.FirstOrDefault();

			if (fitbitAccessToken == null)
			{
				return null;
			}

			// Check if token is expired
			if (fitbitAccessToken.DateTimeAdded.AddSeconds(fitbitAccessToken.ExpiresIn) < DateTime.Now)
			{
				fitbitAccessToken = await RefreshAccessToken(fitbitAccessToken);
			}

			var token = fitbitAccessToken.AccessToken;
			
			using (var client = new HttpClient())
			{
				//https://api.fitbit.com/1/user/-/activities/steps/date/today/1y.json
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await client.GetStringAsync("https://api.fitbit.com/1/user/-/activities/steps/date/today/1y.json");
				JObject responseObject = JObject.Parse(response);

				return Json(responseObject);
			}
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
			await _dataContext.SaveChangesAsync();

			await ExchangeAuthCodeForAccessToken(fitbitAuthCode);			
		}

		private async Task ExchangeAuthCodeForAccessToken(FitbitAuthCode fitbitAuthCode)
		{
			var formUrlEncodedContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("client_id", fitbitAuthCode.FitbitClientDetail.ClientId),
				new KeyValuePair<string, string>("code", fitbitAuthCode.AuthCode),
				new KeyValuePair<string, string>("redirect_uri", _fitbitSettings.CallbackUrl)
			});

			var fitbitAccessToken = await GetAccessOrRefreshAccessToken(fitbitAuthCode, formUrlEncodedContent);

			_dataContext.FitbitAccessTokens.Add(fitbitAccessToken);
			await _dataContext.SaveChangesAsync();
		}

		private async Task<FitbitAccessToken> RefreshAccessToken(FitbitAccessToken fitbitAccessToken)
		{
			var formUrlEncodedContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "refresh_token"),
				new KeyValuePair<string, string>("refresh_token", fitbitAccessToken.RefreshToken)
			});

			var freshFitbitAccessToken = await GetAccessOrRefreshAccessToken(fitbitAccessToken.FitbitAuthCode, formUrlEncodedContent);

			_dataContext.FitbitAccessTokens.Add(freshFitbitAccessToken);
			await _dataContext.SaveChangesAsync();

			return freshFitbitAccessToken;
		}

		private async Task<FitbitAccessToken> GetAccessOrRefreshAccessToken(FitbitAuthCode fitbitAuthCode, FormUrlEncodedContent formUrlEncodedContent)
		{
			using (var httpClient = new HttpClient())
			{
				var clientIdConcatSecret = $"{fitbitAuthCode.FitbitClientDetail.ClientId}:{fitbitAuthCode.FitbitClientDetail.ClientSecret}";
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(clientIdConcatSecret));

				HttpResponseMessage response = await httpClient.PostAsync(_fitbitSettings.AccessAndRefreshUrl, formUrlEncodedContent);
				string responseString = await response.Content.ReadAsStringAsync();

				JObject responseObject = JObject.Parse(responseString);

				// Note: if user cancels the auth process Fitbit returns a 200 response, but the JSON payload is way different.
				var error = responseObject["errors"];
				if (error != null)
				{
					// TODO: Actually should probably raise an exception here maybe?
					return null;
				}

				var fitbitAccessToken = CreateFitbitAccessToken(responseObject);
				fitbitAccessToken.FitbitAuthCode = fitbitAuthCode;
				fitbitAccessToken.DateTimeAdded = DateTime.Now;

				return fitbitAccessToken;
			}
		}

		private FitbitAccessToken CreateFitbitAccessToken(JObject responseObject)
		{
			var fitbitAccessToken = new FitbitAccessToken();

			var temp_access_token = responseObject["access_token"];
			if (temp_access_token != null) fitbitAccessToken.AccessToken = temp_access_token.ToString();

			var temp_expires_in = responseObject["expires_in"];
			if (temp_expires_in != null) fitbitAccessToken.ExpiresIn = Convert.ToInt32(temp_expires_in.ToString());

			var temp_token_type = responseObject["token_type"];
			if (temp_token_type != null) fitbitAccessToken.TokenType = temp_token_type.ToString();

			var temp_refresh_token = responseObject["refresh_token"];
			if (temp_refresh_token != null) fitbitAccessToken.RefreshToken = temp_refresh_token.ToString();

			return fitbitAccessToken;
		}

		private string Base64Encode(string v)
		{
			var byt = System.Text.Encoding.UTF8.GetBytes(v);
			return Convert.ToBase64String(byt);
		}
	}
}
