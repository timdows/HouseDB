using Serilog;
using System;

namespace HouseDB.Core.Exceptions
{
	public class MediatRValidationException : Exception
	{
		public MediatRValidationException(string message) : base(message)
		{
			Log.Warning("{@MediatRValidationException}", new { MediatRValidationException = message });
		}
	}
}
