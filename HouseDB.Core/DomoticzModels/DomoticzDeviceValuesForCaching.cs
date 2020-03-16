using System;

namespace HouseDB.Core.DomoticzModels
{
    public class DomoticzDeviceValuesForCaching
    {
        public long DeviceID { get; set; }
        public decimal CurrentWattValue { get; set; }
        public decimal TodayKwhUsage { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
