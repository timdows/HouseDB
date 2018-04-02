namespace HouseDB.Fitbit
{
	public class FitbitSettings
    {
		/// <summary>
		/// OAuth 2.0 Client ID
		/// </summary>
		public string ClientId { get; set; }
		/// <summary>
		/// Client Secret
		/// </summary>
		public string ClientSecret { get; set; }
		/// <summary>
		/// Callback URL
		/// </summary>
		public string CallbackUrl { get; set; }
		/// <summary>
		/// OAuth 2.0: Authorization URI
		/// </summary>
		public string AuthUrl { get; set; }
		/// <summary>
		/// OAuth 2.0: Access/Refresh Token Request URI
		/// </summary>
		public string AccessAndRefreshUrl { get; set; }
	}
}
