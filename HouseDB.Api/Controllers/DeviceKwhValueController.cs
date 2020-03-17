using HouseDB.Core.UseCases.DomoticzKwhDeviceValue;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers
{
    public class DeviceKwhValueController : HouseDBBaseController
    {
        private readonly IMediator _mediator;

        public DeviceKwhValueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(InsertDomoticzDeviceKwhValuesResponse), 200)]
        public async Task<IActionResult> InsertDomoticzDeviceKwhValues([FromBody] InsertDomoticzDeviceKwhValuesRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetDomoticzDevicesForKwhExportResponse), 200)]
        public async Task<IActionResult> GetDomoticzDevicesForKwhExport([FromBody] GetDomoticzDevicesForKwhExportRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
