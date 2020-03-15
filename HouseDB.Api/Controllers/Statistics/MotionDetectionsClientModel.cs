using HouseDB.Api.Data.Models;
using System.Collections.Generic;

namespace HouseDB.Api.Controllers.Statistics
{
	public class MotionDetectionsClientModel
    {
		public Data.Models.Device Device { get; set; }
		public int Week { get; set; }
		public List<MotionDetection> MotionDetections { get; set; }
	}
}
