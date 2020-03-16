using HouseDB.Core.UseCases.SevenSegment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers
{
    [Route("proxy/Statistics/[action]")]
    public class ProxyController : HouseDBBaseController
    {
        private readonly IMediator _mediator;

        public ProxyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetSevenSegmentResponse), 200)]
        public async Task<IActionResult> GetCurrentUsages()
        {
            var result = await _mediator.Send(new GetSevenSegmentRequest());
            return Ok(result);
        }
    }
}
