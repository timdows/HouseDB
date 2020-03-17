namespace HouseDB.Core.Models
{
    public class DeviceKwhUsage
    {
        public string Name { get; set; }
        public decimal Today { get; set; }
        public decimal ThisWeek { get; set; }
        public decimal LastWeek { get; set; }
        public decimal ThisMonth { get; set; }
        public decimal LastMonth { get; set; }
    }
}
