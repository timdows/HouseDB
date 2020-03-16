using HouseDB.Core.UseCases.SevenSegment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers
{
    public class SevenSegmentController : HouseDBBaseController
    {
        private readonly IMediator _mediator;

        public SevenSegmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetSevenSegmentResponse), 200)]
        public async Task<IActionResult> GetSevenSegment()
        {
            var result = await _mediator.Send(new GetSevenSegmentRequest());
            return Ok(result);
        }
    }
}
