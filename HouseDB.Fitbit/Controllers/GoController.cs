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

		public IActionResult Index(string clientId)
		{
			var scope = "activity";
			var redir = $"{_fitbitSettings.AuthUrl}?response_type=code" +
				$"&client_id={clientId}" +
				$"&display=touch" +
				$"&redirect_uri={_fitbitSettings.CallbackUrl}" +
				$"&scope={scope}" +
				$"&state={clientId}";
			return Redirect(redir);
		}
    }
}
