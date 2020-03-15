using System.Collections.Generic;

namespace HouseDB.Api.Controllers.Exporter
{
	public class DomoticzMotionDetectionClientModel
    {
		public Data.Models.Device Device { get; set; }
		public List<Data.Exporter.DomoticzMotionDetection> MotionDetections { get; set; }
	}
}
