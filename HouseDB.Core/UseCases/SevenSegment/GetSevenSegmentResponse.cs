namespace HouseDB.Core.UseCases.SevenSegment
{
	public class GetSevenSegmentResponse
    {
		public int Watt { get; set; } = 0;
		public decimal Today { get; set; } = 0;
		public decimal ThisWeek { get; set; } = 0;
		public decimal ThisMonth { get; set; } = 0;
		public decimal LastMonth { get; set; } = 0;
	}
}
