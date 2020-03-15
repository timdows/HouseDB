using HouseDB.Core.Settings;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace HouseDB.IS4
{
	public class Config
    {
		public static IEnumerable<ApiResource> GetApiResources(List<IdentityServerApi> identityServerApis)
		{
			foreach (var identityServerApi in identityServerApis)
			{
				yield return new ApiResource(identityServerApi.Name, identityServerApi.DisplayName);
			}
		}

		public static IEnumerable<Client> GetClients(List<IdentityServerApi> identityServerApis)
		{
			foreach (var identityServerApi in identityServerApis)
			{
				foreach (var identityServerClient in identityServerApi.IdentityServerClients)
				{
					yield return new Client
					{
						ClientId = identityServerClient.ClientID,
						AllowedGrantTypes = GrantTypes.ClientCredentials,
						ClientSecrets =
						{
							new Secret(identityServerClient.Secret)
						},
						AllowedScopes = { identityServerApi.Name }
					};
				}
			}
		}
	}
}
