using AutoMapper;
using HouseDB.Core.Exceptions;
using HouseDB.Core.Interfaces;
using HouseDB.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.DomoticzKwhDeviceValue
{
    public class GetDomoticzDevicesForKwhExportHandler : IRequestHandler<GetDomoticzDevicesForKwhExportRequest, GetDomoticzDevicesForKwhExportResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _deviceRepository;

        public GetDomoticzDevicesForKwhExportHandler(
            IMapper mapper,
            IDeviceRepository deviceRepository)
        {
            _mapper = mapper;
            _deviceRepository = deviceRepository;
        }

        public async Task<GetDomoticzDevicesForKwhExportResponse> Handle(GetDomoticzDevicesForKwhExportRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetDomoticzDevicesForKwhExportRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

            var devices = _deviceRepository.GetAllDevicesForKwhExport();
            var deviceDTOs = _mapper.Map<List<DeviceDTO>>(devices);

            return new GetDomoticzDevicesForKwhExportResponse
            {
                Devices = deviceDTOs
            };
        }
    }
}
