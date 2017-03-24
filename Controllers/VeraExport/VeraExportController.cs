using HouseDB.Data;
using HouseDB.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

		public async Task<IActionResult> UploadAsync(ICollection<IFormFile> files)
		{
			_logger.LogWarning("VeraExportController files count {0}", files.Count);

			foreach (var file in files)
			{
				_logger.LogWarning(file.FileName);
				if (file.Length > 0)
				{
					var exportFile = new ExportFile
					{
						ContentType = file.ContentType,
						Length = file.Length,
						DateAdded = DateTime.Now,
						FileName = file.FileName
					};

					//var stream = file.OpenReadStream();
					//using (var memoryStream = new MemoryStream())
					//{
					//	stream.CopyTo(memoryStream);
					//	exportFile.Content = memoryStream.ToArray();
					//}

					using (var fileStream = new FileStream(
						Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("exports", file.FileName)), 
						FileMode.Create))
					{
						await file.CopyToAsync(fileStream);
					}

					_dataContext.Add(exportFile);
				}
			}

			_dataContext.SaveChanges();
			return Json(true);
		}
	}
}
