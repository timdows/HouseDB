using HouseDB.Api.Data;
using HouseDB.Api.Data.Models;
using HouseDB.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers.Fitbit
{
	public static class FitbitHelper
	{ 
		/// <summary>
		/// Get a list of type TEntity from a fitbit API response
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="dataContext"></param>
		/// <param name="fitbitSettings"></param>
		/// <param name="clientId"></param>
		/// <param name="requestUri"></param>
		/// <param name="jsonName"></param>
		/// <returns></returns>
		public static async Task<List<TEntity>> GetResponseList<TEntity>(
			DataContext dataContext, 
			FitbitSettings fitbitSettings, 
			string clientId, 
			string requestUri, 
			string jsonName) where TEntity : class
		{
			var token = await GetAccessToken(dataContext, fitbitSettings, clientId);
			var response = await GetResponseFromApi(token, requestUri);
			var jToken = response[jsonName];
			return jToken.ToObject<List<TEntity>>();
		}

		/// <summary>
		/// Get an access token based on an authorizaion code
		/// </summary>
		/// <param name="fitbitAuthCode"></param>
		/// <returns></returns>
		public static async Task ExchangeAuthCodeForAccessToken(DataContext dataContext, FitbitSettings fitbitSettings, FitbitAuthCode fitbitAuthCode)
		{
			var formUrlEncodedContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("client_id", fitbitAuthCode.FitbitClientDetail.ClientId),
				new KeyValuePair<string, string>("code", fitbitAuthCode.AuthCode),
				new KeyValuePair<string, string>("redirect_uri", fitbitSettings.CallbackUrl)
			});

			var fitbitAccessToken = await GetAccessOrRefreshAccessToken(fitbitAuthCode, fitbitSettings, formUrlEncodedContent);

			dataContext.FitbitAccessTokens.Add(fitbitAccessToken);
			await dataContext.SaveChangesAsync();
		}

		/// <summary>
		/// Gets the accessToken and refreshes it if needed
		/// </summary>
		/// <param name="clientId"></param>
		/// <returns></returns>
		private static async Task<string> GetAccessToken(DataContext dataContext, FitbitSettings fitbitSettings, string clientId)
		{
			var fitbitAccessToken = dataContext.FitbitAccessTokens
				.Include(a_item => a_item.FitbitAuthCode.FitbitClientDetail)
				.Where(a_item => a_item.FitbitAuthCode.FitbitClientDetail.ClientId == clientId)
				.OrderByDescending(a_item => a_item.DateTimeAdded)
				.FirstOrDefault();

			if (fitbitAccessToken == null)
			{
				return null;
			}

			// Check if token should be refreshed
			if (fitbitAccessToken.DateTimeAdded.AddSeconds(fitbitAccessToken.ExpiresIn) <= DateTime.Now)
			{
				fitbitAccessToken = await RefreshAccessToken(dataContext, fitbitSettings, fitbitAccessToken);
			}

			return fitbitAccessToken.AccessToken;
		}

		/// <summary>
		/// Execute requestUri and return a JObject
		/// </summary>
		/// <param name="token"></param>
		/// <param name="requestUri"></param>
		/// <returns></returns>
		private static async Task<JObject> GetResponseFromApi(string token, string requestUri)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				var response = await client.GetStringAsync(requestUri);
				JObject responseObject = JObject.Parse(response);
				return responseObject;
			}
		}

		/// <summary>
		/// Refresh an expired access token with the refresh token
		/// </summary>
		/// <param name="fitbitAccessToken"></param>
		/// <returns></returns>
		private static async Task<FitbitAccessToken> RefreshAccessToken(DataContext dataContext, FitbitSettings fitbitSettings, FitbitAccessToken fitbitAccessToken)
		{
			var formUrlEncodedContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("grant_type", "refresh_token"),
				new KeyValuePair<string, string>("refresh_token", fitbitAccessToken.RefreshToken)
			});

			var freshFitbitAccessToken = await GetAccessOrRefreshAccessToken(fitbitAccessToken.FitbitAuthCode, fitbitSettings, formUrlEncodedContent);

			dataContext.FitbitAccessTokens.Add(freshFitbitAccessToken);
			await dataContext.SaveChangesAsync();

			return freshFitbitAccessToken;
		}

		/// <summary>
		/// Base for executing an http request to the fitbit AccessAndRefreshUrl
		/// </summary>
		/// <param name="fitbitAuthCode"></param>
		/// <param name="formUrlEncodedContent"></param>
		/// <returns></returns>
		private static async Task<FitbitAccessToken> GetAccessOrRefreshAccessToken(FitbitAuthCode fitbitAuthCode, FitbitSettings fitbitSettings, FormUrlEncodedContent formUrlEncodedContent)
		{
			using (var httpClient = new HttpClient())
			{
				var clientIdConcatSecret = $"{fitbitAuthCode.FitbitClientDetail.ClientId}:{fitbitAuthCode.FitbitClientDetail.ClientSecret}";
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Base64Encode(clientIdConcatSecret));

				HttpResponseMessage response = await httpClient.PostAsync(fitbitSettings.AccessAndRefreshUrl, formUrlEncodedContent);
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

		/// <summary>
		/// Creates an FitbitAccessToken from a json object
		/// </summary>
		/// <param name="responseObject"></param>
		/// <returns></returns>
		private static FitbitAccessToken CreateFitbitAccessToken(JObject responseObject)
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

		private static string Base64Encode(string v)
		{
			var byt = System.Text.Encoding.UTF8.GetBytes(v);
			return Convert.ToBase64String(byt);
		}
	}
}
