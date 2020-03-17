using HouseDB.Core.Exceptions;
using HouseDB.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.DomoticzKwhDeviceValue
{
    public class InsertDomoticzDeviceKwhValuesHandler : IRequestHandler<InsertDomoticzDeviceKwhValuesRequest, InsertDomoticzDeviceKwhValuesResponse>
    {
        private readonly IKwhDateUsageRepository _kwhDateUsageRepository;

        public InsertDomoticzDeviceKwhValuesHandler(IKwhDateUsageRepository kwhDateUsageRepository)
        {
            _kwhDateUsageRepository = kwhDateUsageRepository;
        }

        public async Task<InsertDomoticzDeviceKwhValuesResponse> Handle(InsertDomoticzDeviceKwhValuesRequest request, CancellationToken cancellationToken)
        {
            var validator = new InsertDomoticzDeviceKwhValuesRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

            var minDate = request.DomoticzDeviceKwhUsages.Min(item => item.Date);
            var maxDate = request.DomoticzDeviceKwhUsages.Max(item => item.Date);

            var existingDates = _kwhDateUsageRepository.GetDatesBetweenDates(request.DeviceId, minDate, maxDate);

            foreach (var domoticzDeviceKwhUsages in request.DomoticzDeviceKwhUsages)
            {
                // Skip value if it is already in the database or if it is today
                if (existingDates.Any(item => item == domoticzDeviceKwhUsages.Date) || domoticzDeviceKwhUsages.Date == DateTime.Today)
                {
                    continue;
                }

                _kwhDateUsageRepository.Add(new Entities.KwhDateUsage
                {
                    DeviceId = request.DeviceId,
                    Date = domoticzDeviceKwhUsages.Date,
                    Usage = domoticzDeviceKwhUsages.Usage
                });
            }

            await _kwhDateUsageRepository.SaveChangesAsync();

            return new InsertDomoticzDeviceKwhValuesResponse();
        }
    }
}
