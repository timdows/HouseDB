using Microsoft.AspNetCore.Mvc;

namespace HouseDB.Api.Controllers
{
	[Produces("application/json")]
	[ProducesResponseType(500)]
	[Route("api/[controller]/[action]")]
	//[CustomExceptionFilter]
	//[SetRequestPropertiesFilter]
	//[ServiceFilter(typeof(LogUserActivityFilter))]
	//[ServiceFilter(typeof(ValidateCompanyModuleIsActiveFilter))]
	[ApiController]
	//[Authorize]
	public class HouseDBBaseController : ControllerBase
	{
	}
}
