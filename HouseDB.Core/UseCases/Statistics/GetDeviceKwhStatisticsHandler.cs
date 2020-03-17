using HouseDB.Core.Entities;
using HouseDB.Core.Exceptions;
using HouseDB.Core.Extensions;
using HouseDB.Core.Interfaces;
using HouseDB.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.Statistics
{
    public class GetDeviceKwhStatisticsHandler : IRequestHandler<GetDeviceKwhStatisticsRequest, GetDeviceKwhStatisticsResponse>
    {
        private readonly IDomoticzMemoryCache _domoticzMemoryCache;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IKwhDateUsageRepository _kwhDateUsageRepository;

        public GetDeviceKwhStatisticsHandler(
            IDomoticzMemoryCache domoticzMemoryCache,
            IDeviceRepository deviceRepository,
            IKwhDateUsageRepository kwhDateUsageRepository)
        {
            _domoticzMemoryCache = domoticzMemoryCache;
            _deviceRepository = deviceRepository;
            _kwhDateUsageRepository = kwhDateUsageRepository;
        }

        public async Task<GetDeviceKwhStatisticsResponse> Handle(GetDeviceKwhStatisticsRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetDeviceKwhStatisticsRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

            var devices = _deviceRepository.GetAllDevicesForKwhExport()
                .OrderBy(item => item.Name);

            var deviceKwhStatisticsResponse = new GetDeviceKwhStatisticsResponse
            {
                DeviceKwhUsages = new List<DeviceKwhUsage>()
            };

            foreach (var device in devices)
            {
                var deviceKwhUsagea = GetDeviceKwhUsage(device);
                deviceKwhStatisticsResponse.DeviceKwhUsages.Add(deviceKwhUsagea);
            }

            return deviceKwhStatisticsResponse;
        }

        private DeviceKwhUsage GetDeviceKwhUsage(Device device)
        {
            var thisWeeksNumber = DateTime.Today.GetIso8601WeekOfYear();
            var startOfThisWeek = DateTimeExtension.FirstDateOfWeekISO8601(DateTime.Today.Year, thisWeeksNumber);
            var startOfLastWeek = startOfThisWeek.AddDays(-7);
            var endOfLastWeek = startOfThisWeek.AddDays(-1);
            var startOfThisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var startOfLastMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            var endOfLastMonth = startOfLastMonth.AddMonths(1).AddDays(-1);

            var kwhDateUsages = _kwhDateUsageRepository.GetKwhDateUsageBetweenDates(device.Id, startOfLastMonth, DateTime.Today)

            var deviceKwhUsage = new DeviceKwhUsage
            {
                Name = device.Name,
                Today = 0,
                ThisWeek = kwhDateUsages
                    .Where(item => item.Date >= startOfThisWeek && item.Date <= DateTime.Today)
                    .Sum(item => item.Usage),
                LastWeek = kwhDateUsages
                    .Where(item => item.Date >= startOfLastWeek && item.Date <= endOfLastWeek)
                    .Sum(item => item.Usage),
                ThisMonth = kwhDateUsages
                    .Where(item => item.Date >= startOfThisMonth && item.Date <= DateTime.Today)
                    .Sum(item => item.Usage),
                LastMonth = kwhDateUsages
                    .Where(item => item.Date >= startOfLastMonth && item.Date <= endOfLastMonth)
                    .Sum(item => item.Usage)
            };

            var domoticzDeviceValuesForCaching = _domoticzMemoryCache.TryGetDomoticzDeviceValuesForCachingRequest();
            if (domoticzDeviceValuesForCaching != null && domoticzDeviceValuesForCaching.DomoticzDeviceValuesForCachings.Any())
            {
                var cache = domoticzDeviceValuesForCaching.DomoticzDeviceValuesForCachings.SingleOrDefault(item => item.DeviceID == device.Id);
                if (cache != null)
                {
                    deviceKwhUsage.Today = cache.TodayKwhUsage ?? 0;
                    deviceKwhUsage.ThisWeek += cache.TodayKwhUsage ?? 0;
                }
            }

            return deviceKwhUsage;
        }
    }
}
