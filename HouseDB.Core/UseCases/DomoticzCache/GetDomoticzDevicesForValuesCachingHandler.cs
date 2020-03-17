using AutoMapper;
using HouseDB.Core.Exceptions;
using HouseDB.Core.Interfaces;
using HouseDB.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class GetDomoticzDevicesForValuesCachingHandler : IRequestHandler<GetDomoticzDevicesForValuesCachingRequest, GetDomoticzDevicesForValuesCachingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _deviceRepository;

        public GetDomoticzDevicesForValuesCachingHandler(
            IMapper mapper,
            IDeviceRepository deviceRepository)
        {
            _mapper = mapper;
            _deviceRepository = deviceRepository;
        }

        public async Task<GetDomoticzDevicesForValuesCachingResponse> Handle(GetDomoticzDevicesForValuesCachingRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetDomoticzDevicesForValuesCachingRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

            var devices = _deviceRepository.GetAllDevicesForCachingValues();
            var deviceDTOs = _mapper.Map<List<DeviceDTO>>(devices);

            return new GetDomoticzDevicesForValuesCachingResponse
            {
                Devices = deviceDTOs
            };
        }
    }
}
