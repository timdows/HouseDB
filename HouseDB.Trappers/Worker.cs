using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HouseDB.Trappers
{
	public class MySportsApi
	{
		private CookieContainer _cookieJar;
		private HttpClientHandler _handler;
		private HttpClient _client;
		private bool _isLoggedIn;

		public MySportsApi(string baseUri = "https://www.trappers.net")
		{
			_cookieJar = new CookieContainer();
			_handler = new HttpClientHandler
			{
				CookieContainer = _cookieJar,
				UseCookies = true,
			};
			_client = new HttpClient(_handler)
			{
				BaseAddress = new Uri(baseUri)
			};
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<bool> Login(string email, string password)
		{
			var response = await _client.PostAsJsonAsync("/deelnemersite/tres_mijntrapperslogin.jsp", new { email, password });
			_isLoggedIn = response.StatusCode == HttpStatusCode.OK && _cookieJar.GetCookies(_client.BaseAddress).Count == 1;
			return _isLoggedIn;
		}

		//public async Task<User> GetUserData()
		//{
		//	var response = await _client.GetStringAsync("/service/webapi/v2/user/self");
		//	return User.FromJson(response);
		//}

		//public async Task<Activity> GetActivity()
		//{
		//	var response = await _client.GetStringAsync("");
		//	return Activity.FromJson(response);
		//}
	}
}
