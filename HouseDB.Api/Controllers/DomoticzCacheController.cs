using HouseDB.Core.UseCases.DomoticzCache;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers
{
    public class DomoticzCacheController : HouseDBBaseController
    {
        private readonly IMediator _mediator;

        public DomoticzCacheController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(InsertDomoticzDeviceValuesForCachingResponse), 200)]
        public async Task<IActionResult> InsertDomoticzDeviceValuesForCaching([FromBody] InsertDomoticzDeviceValuesForCachingRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(GetDomoticzDevicesForValuesCachingResponse), 200)]
        public async Task<IActionResult> GetDomoticzDevicesForValuesCaching([FromBody] GetDomoticzDevicesForValuesCachingRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
