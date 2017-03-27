using HouseDB.Extensions;
using System;
using System.Collections.Generic;

namespace HouseDB.Data.Models
{
	public class KwhDeviceValue : SqlBase
	{
		public Device Device { get; set; }
		public DateTime DateTime { get; set; }
		public long UnixTimestamp { get; set; }
		public Decimal Value { get; set; }
		public string RawDataLine { get; set; }

		public static KwhDeviceValue Create(string rawDataLine, Device device)
		{
			var split = rawDataLine.Split(',');
			if (split.Length != 2)
			{
				return null;
			}

			var kwhDeviceValue = new KwhDeviceValue
			{
				UnixTimestamp = long.Parse(split[0]),
				Value = decimal.Parse(split[1]),
				RawDataLine = rawDataLine,
				Device = device
			};
			kwhDeviceValue.DateTime = kwhDeviceValue.UnixTimestamp.FromUnixTime();

			return kwhDeviceValue;
		}

		public override bool Equals(object obj)
		{
			KwhDeviceValue kwhDeviceValue = obj as KwhDeviceValue;

			//Check whether the objects are the same object. 
			if (Object.ReferenceEquals(this, kwhDeviceValue)) return true;

			//Check whether the  properties are equal. 
			return 
				this != null &&
				obj != null &&
				UnixTimestamp.Equals(kwhDeviceValue.UnixTimestamp) &&
				Value.Equals(kwhDeviceValue.Value);
		}

		public override int GetHashCode()
		{
			return UnixTimestamp.GetHashCode() ^ Value.GetHashCode();
		}
	}
}
