using HouseDB.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using System.Linq;

namespace HouseDB.Api.Filters
{
	public class ElapsedTimeFilterAttribute : ActionFilterAttribute
	{
		private Stopwatch _timer;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			_timer = Stopwatch.StartNew();
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			if (_timer == null) return;

			_timer.Stop();

			var elapsedTime = new ElapsedTime
			{
				Milliseconds = _timer.ElapsedMilliseconds,
				Seconds = _timer.Elapsed.Seconds
			};

			context.HttpContext.Response.Headers.Add("ElapsedMilliseconds", new string[] { _timer.ElapsedMilliseconds.ToString() });

			// Only execute if this is a jsonRoute
			if (context.RouteData.Routers
				.OfType<Route>()
				.Any(route => route.Name != "jsonRoute"))
			{
				var originalJson = context.Result as JsonResult;

				if (originalJson?.Value is BaseClientModel)
				{
					dynamic clientModel = originalJson.Value;
					clientModel.ElapsedTime = elapsedTime;
				}
			}
		}
	}
}
