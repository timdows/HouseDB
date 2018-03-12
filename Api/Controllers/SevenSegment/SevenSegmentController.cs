using HouseDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HouseDB.Controllers.SevenSegment
{
	[Route("[controller]/[action]")]
	public class SevenSegmentController : HouseDBController
	{
		private readonly IMemoryCache _memoryCache;

		public SevenSegmentController(DataContext dataContext, IMemoryCache memoryCache) : base(dataContext)
		{
			_memoryCache = memoryCache;
		}

		[HttpGet]
		[Produces(typeof(SevenSegmentClientModel))]
		public JsonResult GetClientModel()
		{
			var clientModel = new SevenSegmentClientModel(_dataContext, _memoryCache);
			clientModel.Load();
			return Json(clientModel);
		}
	}
}
