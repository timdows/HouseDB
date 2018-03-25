using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;

namespace HouseDB.Api.Filters
{
	public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private readonly ILogger<CustomExceptionFilterAttribute> _logger;

		public CustomExceptionFilterAttribute(
			ILogger<CustomExceptionFilterAttribute> logger)
		{
			_logger = logger;
		}

		public override void OnException(ExceptionContext context)
		{
			_logger.LogError(context.Exception.Message);

			// Only catch if this is a jsonRoute
			if (context.RouteData.Routers
				.OfType<Route>()
				.Any(route => route.Name != "jsonRoute")) return;

			const bool coreError = true;
			var mvcController = context.ActionDescriptor.RouteValues["controller"];
			var mvcAction = context.ActionDescriptor.RouteValues["action"];
			var errorMessage = $"{context.Exception.Message} {context.Exception.InnerException?.Message}";

			var result = new JsonResult(new { coreError, mvcController, mvcAction, errorMessage });

			//context.ExceptionHandled = true;
			context.Result = new ObjectResult(result)
			{
				StatusCode = (int)HttpStatusCode.InternalServerError
			};
		}
	}
}
