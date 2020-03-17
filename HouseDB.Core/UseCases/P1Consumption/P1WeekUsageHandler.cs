using HouseDB.Core.Exceptions;
using HouseDB.Core.Extensions;
using HouseDB.Core.Interfaces;
using HouseDB.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.P1Consumption
{
	public class P1WeekUsageHandler : IRequestHandler<P1WeekUsageRequest, P1WeekUsageResponse>
    {
		private readonly IP1ConsumptionRepository _p1ConsumptionRepository;

		public P1WeekUsageHandler(IP1ConsumptionRepository p1ConsumptionRepository)
        {
			_p1ConsumptionRepository = p1ConsumptionRepository;
		}

        public async Task<P1WeekUsageResponse> Handle(P1WeekUsageRequest request, CancellationToken cancellationToken)
        {
            var validator = new P1WeekUsageRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

			var p1WeekUsageResponse = new P1WeekUsageResponse
			{
				P1WeekUsages = new List<P1WeekUsage>()
			};

			for (var i = 0; i < request.AmountOfWeeks; i++)
			{
				var date = DateTime.Today.AddDays(-7 * i);
				var weekNumber = date.GetIso8601WeekOfYear();

				var startOfWeek = DateTimeExtension.FirstDateOfWeekISO8601(date.Year, weekNumber);
				var endOfWeek = startOfWeek.AddDays(7);

				var consumption = _p1ConsumptionRepository.GetUsageBetweenDates(startOfWeek, endOfWeek);

				p1WeekUsageResponse.P1WeekUsages.Add(new P1WeekUsage
				{
					Week = weekNumber,
					P1Usage = consumption,
					DisplayText = $"{consumption.ToString("F")} kWh - ({weekNumber}) {startOfWeek.ToString("dd-MM")} - {endOfWeek.ToString("dd-MM")}"
				});
			}

			return p1WeekUsageResponse;
		}
    }
}
