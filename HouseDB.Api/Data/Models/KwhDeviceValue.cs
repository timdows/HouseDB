using HouseDB.Core.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseDB.Api.Data.Models
{
	public class KwhDeviceValue : SqlBase
	{
		public Device Device { get; set; }
		public DateTime DateTime { get; set; }
		public long UnixTimestamp { get; set; }
		public Decimal Value { get; set; }
		public string RawDataLine { get; set; }

		[ForeignKey("Device")]
		public long DeviceID { get; set; }

		public static KwhDeviceValue Create(string rawDataLine, Device device)
		{
			var split = rawDataLine.Split(',');
			if (split.Length != 2)
			{
				return null;
			}

			if (!long.TryParse(split[0], out long unixTimestamp) || !decimal.TryParse(split[1], out decimal value))
			{
				return null;
			}

			var kwhDeviceValue = new KwhDeviceValue
			{
				UnixTimestamp = unixTimestamp,
				Value = value,
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
