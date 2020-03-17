using System;

namespace HouseDB.Core.Entities
{
    public class KwhDateUsage : SqlBase
    {
        public int DeviceId { get; set; }
        public Device Device { get; set; }

        public DateTime Date { get; set; }
        public decimal Usage { get; set; }
    }
}
