using HouseDB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HouseDB.Controllers.VeraExport
{
	public class VeraExportController : HouseDBController
	{
		private readonly ILogger<VeraExportController> _logger;

		public VeraExportController(
			DataContext dataContext,
			ILogger<VeraExportController> logger
			) : base(dataContext)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Upload(ICollection<IFormFile> files)
		{
			_logger.LogWarning("Files count {0}", files.Count);

			var uploads = Path.Combine(Path.GetTempPath(), "uploads");
			foreach (var file in files)
			{
				_logger.LogWarning(file.Name);
				_logger.LogWarning(file.FileName);
				if (file.Length > 0)
				{
					using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
					{
						await file.CopyToAsync(fileStream);
					}
				}
			}
			return Json(true);
		}
	}
}
