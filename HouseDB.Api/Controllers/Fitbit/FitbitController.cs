using HouseDB.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HouseDB.Api.Controllers.Fitbit
{
	[Route("[controller]/[action]")]
	public class FitbitController : HouseDBController
	{
		public FitbitController(DataContext dataContext) : base(dataContext)
		{
		}

		[HttpPost]
		public void InsertCallbackCode([FromBody] string code)
		{
			_dataContext.FitbitAuthCodes.Add(new Data.Models.FitbitAuthCode
			{
				AuthCode = code,
				DateTimeAdded = DateTime.Now
			});

			_dataContext.SaveChangesAsync();
		}
	}
}
