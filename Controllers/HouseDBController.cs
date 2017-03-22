using Microsoft.AspNetCore.Mvc;
using HouseDB.Data;

namespace HouseDB.Controllers
{
	public class HouseDBController : Controller
	{
		public HouseDBController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		protected DataContext _dataContext { get; set; }
	}
}
