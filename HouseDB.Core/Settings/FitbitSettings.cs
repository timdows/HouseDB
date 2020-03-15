namespace HouseDB.Core.Settings
{
	public class FitbitSettings
    {
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
