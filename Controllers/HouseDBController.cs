using Microsoft.AspNetCore.Mvc;
using HouseDB.Data;
using HouseDB.Filters;

namespace HouseDB.Controllers
{
	[ElapsedTimeFilter]
	public class HouseDBController : Controller
	{
		public HouseDBController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		protected DataContext _dataContext { get; set; }
	}
}
