namespace HouseDB.Controllers.SevenSegment
{
	public class PowerImportValueClientModel
    {
		public string Label { get; set; }
		public decimal Min { get; set; }
		public decimal Max { get; set; }
		public string Type
		{
			get
			{
				if (Label.Equals("Powermeter ImportRate2 - KWH"))
				{
					return "High";
				}
				return "Low";
			}
		}
	}
}
