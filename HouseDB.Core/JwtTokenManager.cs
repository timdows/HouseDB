using HouseDB.Core.Settings;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace HouseDB.Core
{
	public class JwtTokenManager
    {
		private TokenResponse _tokenResponse;
		private DateTime _tokenEpoch;

		public async Task<string> GetToken(HouseDBSettings houseDBSettings)
		{
			if (_tokenResponse == null)
			{
				_tokenResponse = await GetJWTAccessToken(houseDBSettings);
				_tokenEpoch = DateTime.Now;
			}
			else
			{
				// Check if token is expiered (in seconds)
				if ((DateTime.Now - _tokenEpoch).TotalSeconds > _tokenResponse.ExpiresIn)
				{
					_tokenResponse = await GetJWTAccessToken(houseDBSettings);
					_tokenEpoch = DateTime.Now;
				}
			}

			return _tokenResponse.AccessToken;
		}

		private static async Task<TokenResponse> GetJWTAccessToken(HouseDBSettings houseDBSettings)
		{
			var disco = await DiscoveryClient.GetAsync(houseDBSettings.IS4Url);

			// request token
			var tokenClient = new TokenClient(disco.TokenEndpoint, houseDBSettings.ClientID, houseDBSettings.Password);
			var tokenResponse = await tokenClient.RequestClientCredentialsAsync(houseDBSettings.Scope);

			return tokenResponse;
		}
	}
}
