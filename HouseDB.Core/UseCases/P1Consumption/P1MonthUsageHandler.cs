using HouseDB.Core.Exceptions;
using HouseDB.Core.Interfaces;
using HouseDB.Core.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.P1Consumption
{
	public class P1MonthUsageHandler : IRequestHandler<P1MonthUsageRequest, P1MonthUsageResponse>
    {
		private readonly IP1ConsumptionRepository _p1ConsumptionRepository;

		public P1MonthUsageHandler(IP1ConsumptionRepository p1ConsumptionRepository)
        {
			_p1ConsumptionRepository = p1ConsumptionRepository;
		}

        public async Task<P1MonthUsageResponse> Handle(P1MonthUsageRequest request, CancellationToken cancellationToken)
        {
            var validator = new P1MonthUsageRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new MediatRValidationException(result.ToString());
            }

			var p1MonthUsageResponse = new P1MonthUsageResponse
			{
				P1MonthUsages = new List<P1MonthUsage>()
			};

			for (var i = 0; i < request.AmountOfMonths; i++)
			{
				var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-i);
				var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

				var consumption = _p1ConsumptionRepository.GetUsageBetweenDates(startOfMonth, endOfMonth);

				p1MonthUsageResponse.P1MonthUsages.Add(new P1MonthUsage
				{
					Year = startOfMonth.Year,
					Month = startOfMonth.Month,
					P1Usage = consumption,
					DisplayText = $"{consumption.ToString("F")} kWh - ({startOfMonth.Year}-startOfMonth.Month)"
				});
			}

			return p1MonthUsageResponse;
		}
    }
}
