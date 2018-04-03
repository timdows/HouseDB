using HouseDB.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HouseDB.Fitbit.Controllers
{
	public class GoController : Controller
    {
		private readonly FitbitSettings _fitbitSettings;

		public GoController(IOptions<FitbitSettings> fitbitSettings)
		{
			_fitbitSettings = fitbitSettings.Value;
		}

		public IActionResult Index()
		{
			var clientId = _fitbitSettings.ClientId;
			var redirectUrl = _fitbitSettings.CallbackUrl;
			var scope = "activity";
			var redir = $"https://www.fitbit.com/oauth2/authorize?response_type=code&client_id={clientId}&display=touch&redirect_uri={redirectUrl}&scope={scope}";
			return Redirect(redir);
		}
    }
}
