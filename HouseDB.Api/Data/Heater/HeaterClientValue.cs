﻿using System;

namespace HouseDB.Api.Data.Heater
{
	public class HeaterClientValue
	{
		public DateTime Date { get; set; }
		public int PureValue { get; set; }
		public int SumValue { get; set; }
		public int DifferenceValue { get; set; }
	}
}
