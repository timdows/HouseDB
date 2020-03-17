using HouseDB.Core.DomoticzModels;
using HouseDB.Core.Exceptions;
using HouseDB.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class InsertDomoticzDeviceValuesForCachingHandler : IRequestHandler<InsertDomoticzDeviceValuesForCachingRequest, InsertDomoticzDeviceValuesForCachingResponse>
    {
        private readonly IDomoticzMemoryCache _domoticzMemoryCache;

        public InsertDomoticzDeviceValuesForCachingHandler(IDomoticzMemoryCache domoticzMemoryCache)
        {
            _domoticzMemoryCache = domoticzMemoryCache;
        }

        public async Task<InsertDomoticzDeviceValuesForCachingResponse> Handle(InsertDomoticzDeviceValuesForCachingRequest request, CancellationToken cancellationToken)
        {
            var validator = new InsertDomoticzDeviceValuesForCachingRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

            _domoticzMemoryCache.SetDomoticzDeviceValues($"{nameof(DomoticzDeviceValuesForCaching)}_{request.DomoticzDeviceValuesForCaching.DeviceID}", request.DomoticzDeviceValuesForCaching);

            return new InsertDomoticzDeviceValuesForCachingResponse();
        }
    }
}
