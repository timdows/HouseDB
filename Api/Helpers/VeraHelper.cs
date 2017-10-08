using HouseDB.Data.DataMine;
using HouseDB.Data.Settings;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseDB.Helpers
{
	public class VeraHelper
	{
		public enum SpanPeriod
		{
			Day = 1,
			Hour,
			Minute
		};

		public enum ReturnType
		{
			MinimumValue = 1,
			MaximumValue
		};

		public static async Task<DataMineSingleReturnValue> GetSingleValueByTime(
			VeraSettings veraSettings,
			int channel,
			long unixTime,
			SpanPeriod spanPeriod)
		{
			long stop = 0;
			switch (spanPeriod)
			{
				case SpanPeriod.Minute:
					stop = unixTime + 60;
					break;
				case SpanPeriod.Hour:
					stop = unixTime + 3600;
					break;
				case SpanPeriod.Day:
					stop = unixTime + 86400;
					break;
			}

			var url =
				$"http://{veraSettings.VeraIpAddress}/port_3480/data_request?id=lr_dmData&start={--unixTime}&stop={++stop}&channel1={channel}";

			using (var webClient = new HttpClient())
			{
				var result = await webClient.GetStringAsync(url);
				var json = JObject.Parse(result);

				return new DataMineSingleReturnValue
				{
					Url = url,
					StartValue = GetValidValue(json, 0, ReturnType.MinimumValue),
					EndValue = GetValidValue(json, 0, ReturnType.MaximumValue)
				};
			}
		}

		public static async Task<DataMineDoubleReturnValue> GetMultipleValuesByTime(
			VeraSettings veraSettings,
			int channel1,
			int channel2,
			long unixTime,
			SpanPeriod spanPeriod)
		{
			long stop = 0;
			switch (spanPeriod)
			{
				case SpanPeriod.Minute:
					stop = unixTime + 60;
					break;
				case SpanPeriod.Hour:
					stop = unixTime + 3600;
					break;
				case SpanPeriod.Day:
					stop = unixTime + 86400;
					break;
			}

			var url =
				$"http://{veraSettings.VeraIpAddress}/port_3480/data_request?id=lr_dmData&start={--unixTime}&stop={++stop}&channel1={channel1}&channel2={channel2}";

			using (var webClient = new HttpClient())
			{
				var result = await webClient.GetStringAsync(url);
				var json = JObject.Parse(result);

				return new DataMineDoubleReturnValue
				{
					Json = json,
					Url = url,
					HighStart = GetValidValue(json, 0, ReturnType.MinimumValue),
					HighEnd = GetValidValue(json, 0, ReturnType.MaximumValue),
					LowStart = GetValidValue(json, 1, ReturnType.MinimumValue),
					LowEnd = GetValidValue(json, 1, ReturnType.MaximumValue)
				};
			}
		}

		//public static async Task<DatamineAll> GetAllValuesByTime(
		//	VeraSettings veraSettings,
		//	int channel,
		//	long unixTime,
		//	SpanPeriod spanPeriod)
		//{
		//	long stop = 0;
		//	switch (spanPeriod)
		//	{
		//		case SpanPeriod.Minute:
		//			stop = unixTime + 60;
		//			break;
		//		case SpanPeriod.Hour:
		//			stop = unixTime + 3600;
		//			break;
		//		case SpanPeriod.Day:
		//			stop = unixTime + 86400;
		//			break;
		//	}

		//	var url =
		//		$"{veraSettings.Server}/port_3480/data_request?id=lr_dmData&start={unixTime}&stop={stop}&channel1={channel}";

		//	using (var webClient = new HttpClient())
		//	{
		//		var result = await webClient.GetStringAsync(url);
		//		var datamineAll = JsonConvert.DeserializeObject<DatamineAll>(result);
		//		datamineAll.Url = url;

		//		return datamineAll;
		//	}
		//}

		private static decimal GetValidValue(JObject json, int serie, ReturnType returnType)
		{
			string objectKey;
			switch (returnType)
			{
				case ReturnType.MinimumValue:
					objectKey = "min";
					break;
				case ReturnType.MaximumValue:
					objectKey = "max";
					break;
				default:
					return 0;
			}

			decimal retValue;
			if (decimal.TryParse(json["series"][serie][objectKey].ToString(), out retValue))
			{
				if (retValue == 9999999999 || retValue == -9999999999)
				{
					return 0;
					// Try to get the first data serie, maybe not as than the current value is returned?
					var dataJson = json["series"][serie]["data"];
					return VeraHelper.GetFirstDataValue(dataJson, 0);
				}

				return retValue;
			}

			return 0;
		}

		private static decimal GetFirstDataValue(JToken json, int index)
		{
			decimal retValue;
			// Inside the index, array 0 is unix timestamp and 1 is the value
			if (decimal.TryParse(json[index][1].ToString(), out retValue))
			{
				if (retValue != 0)
				{
					return retValue;
				}

				return VeraHelper.GetFirstDataValue(json, ++index);
			}

			return 0;
		}
	}
}
