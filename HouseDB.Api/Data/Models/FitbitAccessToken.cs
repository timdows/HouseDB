﻿using System;

namespace HouseDB.Api.Data.Models
{
	public class FitbitAccessToken : SqlBase
    {
		public FitbitAuthCode FitbitAuthCode { get; set; }
		public string AccessToken { get; set; }
		public DateTime DateTimeAdded { get; set; }
	}
}