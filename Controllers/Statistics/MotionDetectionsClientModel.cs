using HouseDB.Data.Models;
using System.Collections.Generic;

namespace HouseDB.Controllers.Statistics
{
	public class MotionDetectionsClientModel
    {
		public Data.Models.Device Device { get; set; }
		public int Week { get; set; }
		public List<MotionDetection> MotionDetections { get; set; }
	}
}
