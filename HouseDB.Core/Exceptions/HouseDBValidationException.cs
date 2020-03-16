using Serilog;
using System;

namespace HouseDB.Core.Exceptions
{
	public class HouseDBValidationException : Exception
	{
		public HouseDBValidationException(string message) : base(message)
		{
			Log.Warning("{@HouseDBValidationException}", new { HouseDBValidationException = message });
		}
	}
}
