namespace HouseDB.Core.Models
{
    public class DeviceKwhUsage
    {
        public string Name { get; set; }
        
        public decimal Today { get; set; }
        public string TodayFormatted => $"{decimal.Round(Today, 2)}";

        public decimal ThisWeek { get; set; }
        public string ThisWeekFormatted => $"{decimal.Round(ThisWeek, 2)}";

        public decimal LastWeek { get; set; }
        public string LastWeekFormatted => $"{decimal.Round(LastWeek, 2)}";

        public decimal ThisMonth { get; set; }
        public string ThisMonthFormatted => $"{decimal.Round(ThisMonth, 2)}";

        public decimal LastMonth { get; set; }
        public string TLastMonthFormatted => $"{decimal.Round(LastMonth, 2)}";
    }
}
