using HouseDB.Data;
using HouseDB.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseDB.Controllers
{
	[ElapsedTimeFilter]
	[Authorize]
	public class HouseDBController : Controller
	{
		public HouseDBController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		protected DataContext _dataContext { get; set; }
	}
}
