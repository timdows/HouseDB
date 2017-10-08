﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace HouseDB.Data
{
	public class EFConfigSource : IConfigurationSource
	{
		private readonly Action<DbContextOptionsBuilder> _optionsAction;

		public EFConfigSource(Action<DbContextOptionsBuilder> optionsAction)
		{
			_optionsAction = optionsAction;
		}

		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new EFConfigProvider(_optionsAction);
		}
	}
}