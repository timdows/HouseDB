using HouseDB.Core.Models;
using System.Collections.Generic;

namespace HouseDB.Core.UseCases.Statistics
{
    public class GetDeviceKwhStatisticsResponse
    {
        public List<DeviceKwhUsage> DeviceKwhUsages { get; set; }
    }
}
