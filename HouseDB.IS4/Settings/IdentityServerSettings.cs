using System.Collections.Generic;

namespace HouseDB.IS4.Settings
{
	public class IdentityServerSettings
    {
		public List<IdentityServerApi> IdentityServerApis { get; set; }
		public CertificateSettings CertificateSettings { get; set; }
	}

	public class IdentityServerApi
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public List<IdentityServerClient> IdentityServerClients { get; set; }
	}

	public class IdentityServerClient
	{
		public string ClientID { get; set; }
		public string Secret { get; set; }
	}

	public class CertificateSettings
	{
		public string FileName { get; set; }
		public string Password { get; set; }
	}
}
