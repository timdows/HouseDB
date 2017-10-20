using Exporter.Models.Settings;
using IdentityModel.Client;
using System.Threading.Tasks;

namespace Exporter.Models
{
	public class JwtTokenManager
    {
		private TokenResponse TokenResponse { get; set; }

		public async Task<string> GetToken(HouseDBSettings houseDBSettings)
		{
			if (TokenResponse == null)
			{
				TokenResponse = await GetJWTAccessToken(houseDBSettings);
			}
			else
			{
				// Check if token is expiered (in seconds)
				if (TokenResponse.ExpiresIn < 10 * 60)
				{
					TokenResponse = await GetJWTAccessToken(houseDBSettings);
				}
			}

			return TokenResponse.AccessToken;
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
