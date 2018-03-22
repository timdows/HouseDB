using HouseDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers.Statistics
{
	public class LastMotionDetectionsClientModel
	{
		public List<LastMotionDetectionDetail> LastMotionDetectionDetails { get; set; } = new List<LastMotionDetectionDetail>();

		public void Load(DataContext dataContext)
		{
			var motionDetectionDevices = dataContext.Devices
				.Where(a_item => a_item.DomoticzMotionDetectionIdx != 0)
				.ToList();

			foreach(var motionDetectionDevice in motionDetectionDevices)
			{
				var detections = dataContext.MotionDetections
					.Where(a_item => a_item.DeviceID == motionDetectionDevice.ID && 
									 a_item.Status &&
									 a_item.DateTimeDetection >= DateTime.Today)
					.OrderByDescending(a_item => a_item.DateTimeDetection)
					.ToList();

				var lastMotionDetectionDetail = new LastMotionDetectionDetail
				{
					DeviceName = motionDetectionDevice.Name,
					DetectionsToday = detections.Count
				};

				if (detections.Count > 2)
				{
					lastMotionDetectionDetail.LastDetection = detections[0].DateTimeDetection;
					lastMotionDetectionDetail.PreviousDetection = detections[1].DateTimeDetection;
				}
				else if (detections.Count == 1)
				{
					lastMotionDetectionDetail.LastDetection = detections[0].DateTimeDetection;
				}

				LastMotionDetectionDetails.Add(lastMotionDetectionDetail);
			}
		}
    }

	public class LastMotionDetectionDetail
	{
		public string DeviceName { get; set; }
		public DateTime? LastDetection { get; set; }
		public DateTime? PreviousDetection { get; set; }
		public int DetectionsToday { get; set; }
	}
}
