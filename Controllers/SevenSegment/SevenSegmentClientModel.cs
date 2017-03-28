using HouseDB.Data;
using HouseDB.Data.Settings;
using HouseDB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseDB.Controllers.SevenSegment
{
	public class SevenSegmentClientModel
	{
		//private const string CacheKey = "SevenSegment";

		public string Watt { get; set; }
		public string LastWeekTotal { get; set; }
		public string ThisWeekTotal { get; set; }
		public string LastMonthTotal { get; set; }
		public string ThisMonthTotal { get; set; }

		//private readonly IMemoryCache _memoryCache;
		private readonly DataContext _dataContext;
		private readonly VeraSettings _veraSettings;
		private readonly DataMineSettings _dataMineSettings;

		public SevenSegmentClientModel(
			DataContext dataContext,
			//IMemoryCache memoryCache,
			VeraSettings veraSettings,
			DataMineSettings dataMineSettings
			)
		{
			_dataContext = dataContext;
			//_memoryCache = memoryCache;
			_veraSettings = veraSettings;
			_dataMineSettings = dataMineSettings;
		}

		public async Task Load()
		{
			//var valuesNow = _memoryCache.Get(SevenSegmentClientModel.CacheKey) as DatamineDoubleReturnValue;
			//if (valuesNow == null)
			//{
				//var valuesNow = await VeraHelper.GetMultipleValuesByTime(
				//	_veraSettings,
				//	Device.Get(_dataContext, _dataMineSettings.PowerImport1Channel).DataMineChannel,
				//	Device.Get(_dataContext, _dataMineSettings.PowerImport2Channel).DataMineChannel,
				//	DateTime.Today.ToUnixTime(),
				//	VeraHelper.SpanPeriod.Day);

				// As the month or week can start on a bankholiday (or any other reason why high is 0) get the next valid high value
				//if (valuesNow.HighEnd == 0)
				//{
				//	valuesNow.HighEnd = KwhDayReadingHelper.GetValidHighStart(_dataContext, DateTime.Now, false).HighEnd;
				//}

				//_memoryCache.Set(
				//	SevenSegmentClientModel.CacheKey,
				//	valuesNow,
				//	new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(5)));
			//}

			//var lastWeek = KwhDayReadingHelper.GetKwhWeekReading(
			//	_dataContext,
			//	DateTime.Now.AddDays(-7).Year,
			//	DateTime.Now.AddDays(-7).GetIso8601WeekOfYear());
			//var lastMonth = KwhDayReadingHelper.GetKwhMonthReading(
			//	_dataContext,
			//	DateTime.Now.AddMonths(-1).Year,
			//	DateTime.Now.AddMonths(-1).Month);

			//var thisWeekStart = new KwhDayReading();
			//// As the values are only in the database the next day, check if we have to overwrite values if it is monday
			//if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
			//{
			//	thisWeekStart.LowStart = valuesNow.LowStart;
			//	thisWeekStart.HighStart = valuesNow.HighStart;
			//	thisWeekStart.Date = DateTime.Now;
			//}
			//else
			//{
			//	thisWeekStart = KwhDayReadingHelper.GetKwhDayReading(
			//		_dataContext,
			//		DateTime.Now.StartOfWeek(DayOfWeek.Monday));
			//}

			//var thisMonthStart = new KwhDayReading();
			//if (DateTime.Today.Day == 1)
			//{
			//	thisMonthStart.LowStart = valuesNow.LowStart;
			//	thisMonthStart.HighStart = valuesNow.HighStart;
			//}
			//else
			//{
			//	thisMonthStart = KwhDayReadingHelper.GetKwhDayReading(
			//		_dataContext,
			//		new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
			//}


			//// As the month or week can start on a bankholiday (or any other reason why high is 0) get the next valid high value
			//if (thisMonthStart.HighStart == 0)
			//{
			//	thisMonthStart.HighStart = KwhDayReadingHelper.GetValidHighStart(_dataContext, thisMonthStart.Date, false).HighStart;
			//}
			//if (thisMonthStart.LowStart == 0)
			//{
			//	thisMonthStart.LowStart = KwhDayReadingHelper.GetValidLowStart(_dataContext, thisMonthStart.Date, false).LowStart;
			//}
			//if (thisWeekStart.HighStart == 0)
			//{
			//	thisWeekStart.HighStart = KwhDayReadingHelper.GetValidHighStart(_dataContext, thisWeekStart.Date, false).HighStart;
			//}
			//if (thisWeekStart.LowStart == 0)
			//{
			//	thisWeekStart.LowStart = KwhDayReadingHelper.GetValidLowStart(_dataContext, thisWeekStart.Date, false).LowStart;
			//}

			//var numberFormatInfo = new NumberFormatInfo
			//{
			//	NumberDecimalSeparator = ".",
			//	NumberGroupSeparator = string.Empty
			//};

			//this.LastWeekTotal = lastWeek
			//	.ToString(numberFormatInfo)
			//	.Substring(0, 5);
			//this.ThisWeekTotal = (valuesNow.HighEnd - thisWeekStart.HighStart + valuesNow.LowEnd - thisWeekStart.LowStart)
			//	.ToString(numberFormatInfo)
			//	.Substring(0, 5);
			//this.LastMonthTotal = lastMonth
			//	.ToString(numberFormatInfo)
			//	.Substring(0, 5);
			//this.ThisMonthTotal = (valuesNow.HighEnd - thisMonthStart.HighStart + valuesNow.LowEnd - thisMonthStart.LowStart)
			//	.ToString(numberFormatInfo)
			//	.Substring(0, 5);

			//var url = $"{_veraSettings.Server}{_veraSettings.RequestString}{_veraSettings.WattChannel}";

			//using (var webClient = new HttpClient())
			//{
			//	var result = await webClient.GetStringAsync(url);
			//	var json = JObject.Parse(result);
			//	this.Watt =
			//		json[$"Device_Num_{_veraSettings.WattChannel}"]["states"][0]["value"].ToString();
			//}

		}
	}
}
