using HouseDB.Core.UseCases.P1Consumption;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers
{
    public class P1ConsumptionController : HouseDBBaseController
    {
        private readonly IMediator _mediator;

        public P1ConsumptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(InsertP1ConsumptionResponse), 200)]
        public async Task<IActionResult> InsertP1Consumption([FromBody] InsertP1ConsumptionRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(P1WeekUsageResponse), 200)]
        public async Task<IActionResult> P1WeekUsage([FromQuery] int amountOfWeeks)
        {
            var request = new P1WeekUsageRequest
            {
                AmountOfWeeks = amountOfWeeks
            };
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
