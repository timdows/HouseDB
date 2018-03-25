namespace HouseDB.Api.Controllers
{
	public class BaseClientModel
	{
		public ElapsedTime ElapsedTime { get; set; }
	}

	public class ElapsedTime
	{
		public long Milliseconds { get; set; }
		public int Seconds { get; set; }
	}
}
