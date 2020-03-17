using HouseDB.Core.UseCases.Statistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HouseDB.Api.Controllers
{
    public class StatisticsController : HouseDBBaseController
    {
        private readonly IMediator _mediator;

        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetDeviceKwhStatisticsResponse), 200)]
        public async Task<IActionResult> GetDeviceKwhStatistics()
        {
            var result = await _mediator.Send(new GetDeviceKwhStatisticsRequest());
            return Ok(result);
        }
    }
}
