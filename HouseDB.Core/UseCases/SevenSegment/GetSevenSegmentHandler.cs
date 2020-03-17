using HouseDB.Core.Exceptions;
using HouseDB.Core.Extensions;
using HouseDB.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.SevenSegment
{
    public class GetSevenSegmentHandler : IRequestHandler<GetSevenSegmentRequest, GetSevenSegmentResponse>
    {
        private readonly IDomoticzMemoryCache _domoticzMemoryCache;
        private readonly IP1ConsumptionRepository _p1ConsumptionRepository;

        public GetSevenSegmentHandler(
            IDomoticzMemoryCache domoticzMemoryCache, 
            IP1ConsumptionRepository p1ConsumptionRepository)
        {
            _domoticzMemoryCache = domoticzMemoryCache;
            _p1ConsumptionRepository = p1ConsumptionRepository;
        }

        public async Task<GetSevenSegmentResponse> Handle(GetSevenSegmentRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetSevenSegmentRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

            var getSevenSegmentResponse = new GetSevenSegmentResponse();

            var cachedInsertDomoticzDeviceValuesForCachingRequest = _domoticzMemoryCache.TryGetDomoticzDeviceValuesForCachingRequest();
            if (cachedInsertDomoticzDeviceValuesForCachingRequest != null)
            {
                var p1Values = cachedInsertDomoticzDeviceValuesForCachingRequest.P1Values;
                if (p1Values != null && p1Values.CurrentWattValue.HasValue)
                {
                    getSevenSegmentResponse.Watt = Convert.ToInt32(p1Values.CurrentWattValue.Value);
                }
                if (p1Values != null && p1Values.TodayKwhUsage.HasValue)
                {
                    getSevenSegmentResponse.Today = Math.Round(p1Values.TodayKwhUsage.Value, 2);
                }
            }

            var domoticzP1Consumptions = _p1ConsumptionRepository.GetUntillLastMonth();

            // Get working dates
            var thisWeekMonday = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
            var thisWeekSunday = thisWeekMonday.AddDays(6);
            var thisMonthFirstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var thisMonthLastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            var previousMonthFirstDay = thisMonthFirstDay.AddMonths(-1);
            var previousMonthLastDay = thisMonthFirstDay.AddDays(-1);

            // Calculate week values
            getSevenSegmentResponse.ThisWeek = (decimal)Math.Round(domoticzP1Consumptions
                .Where(a_item => a_item.Date >= thisWeekMonday &&
                                 a_item.Date <= thisWeekSunday)
                .Sum(a_item => a_item.DayUsage), 2) + getSevenSegmentResponse.Today;

            // Calculate Month values
            getSevenSegmentResponse.ThisMonth = (decimal)Math.Round(domoticzP1Consumptions
                .Where(a_item => a_item.Date >= thisMonthFirstDay &&
                                 a_item.Date <= thisMonthLastDay)
                .Sum(a_item => a_item.DayUsage), 2);

            getSevenSegmentResponse.LastMonth = (decimal)Math.Round(domoticzP1Consumptions
                .Where(a_item => a_item.Date >= previousMonthFirstDay &&
                                 a_item.Date <= previousMonthLastDay)
                .Sum(a_item => a_item.DayUsage), 2);

            return getSevenSegmentResponse;
        }
    }
}
