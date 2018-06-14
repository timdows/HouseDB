using HouseDB.Api.Data;
using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers.Trappers
{
	[Route("[controller]/[action]")]
	public class TrappersController : HouseDBController
	{
		private readonly TrappersSettings _trappersSettings;
		private CookieContainer _cookieJar;
		private HttpClientHandler _handler;
		private HttpClient _client;
		private bool _isLoggedIn;
		private Cookie _loginCookie;

		public TrappersController(
			DataContext dataContext,
			IOptions<TrappersSettings> trappersSettings) : base(dataContext)
		{
			_trappersSettings = trappersSettings.Value;

			_cookieJar = new CookieContainer();
			_handler = new HttpClientHandler
			{
				CookieContainer = _cookieJar,
				UseCookies = true,
			};
			_client = new HttpClient(_handler)
			{
				BaseAddress = new Uri(_trappersSettings.BaseUrl)
			};
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task GetThings()
		{
			var a = await Login();
			await GetTransactions();
		}

		private async Task<bool> Login()
		{
			var loginRequest = new LoginRequest
			{
				Event = "Log-in",
				LoginId = _trappersSettings.LoginId,
				Password = _trappersSettings.Password
			};
			var request = CreateHttpRequestMessage(
				$"{_trappersSettings.BaseUrl}/deelnemersite/tres_mijntrapperslogin.jsp", 
				loginRequest,
				string.Empty);
			var response = await _client.SendAsync(request);

			var cookies = _cookieJar.GetCookies(_client.BaseAddress);
			foreach (Cookie cookie in cookies)
			{
				if (cookie.Name.Equals("JSESSIONID", StringComparison.CurrentCultureIgnoreCase))
				{
					_loginCookie = cookie;
				}
			}

			_isLoggedIn = response.StatusCode == HttpStatusCode.OK && cookies.Count == 1;
			return _isLoggedIn;
		}

		private async Task GetTransactions()
		{
			//var transactionsRequest = new TransactionsRequest
			//{
			//	Event = "Zoek",
			//	BeginDatum = "01-05-2018",
			//	EindDatum = "01-06-2018"
			//};
			//var request = CreateHttpRequestMessage(
			//	$"{_trappersSettings.BaseUrl}/deelnemersite/tres_mijntrapperstransacties.jsp",
			//	transactionsRequest,
			//	_loginCookie.ToString());
			////request./*cook*/
			//var response = await _client.SendAsync(request);

			//var x = await response.Content.ReadAsStringAsync();

			var content = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("begindatum", "01-06-2018^"),
				new KeyValuePair<string, string>("einddatum", "14-06-2018^"),
				new KeyValuePair<string, string>("event", "Zoek")
			});
			_client.DefaultRequestHeaders.Add("cookie", $"JSESSIONID={_loginCookie.Value}");
			var response = await _client.PostAsync($"{_trappersSettings.BaseUrl}/deelnemersite/tres_mijntrapperstransacties.jsp", content);
			var x = await response.Content.ReadAsStringAsync();
		}

		private HttpRequestMessage CreateHttpRequestMessage(string requestUri, object requestObject, string cookie)
		{
			var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
			if (cookie != string.Empty)
			{
				request.Headers.Add("cookie", cookie);
			}
			var json = JsonConvert.SerializeObject(requestObject);
			request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			return request;
		}

		

	}

	internal class LoginRequest
	{
		public string Event { get; set; }
		public string LoginId { get; set; }
		public string Password { get; set; }
	}

	internal class TransactionsRequest
	{
		public string Event { get; set; }
		public string BeginDatum { get; set; }
		public string EindDatum { get; set; }
	}
}
