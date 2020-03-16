using HouseDB.Core.Exceptions;
using HouseDB.Core.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HouseDB.Core.UseCases.P1Consumption
{
    public class InsertP1ConsumptionHandler : IRequestHandler<InsertP1ConsumptionRequest, InsertP1ConsumptionResponse>
    {
        private readonly IP1ConsumptionRepository _p1ConsumptionRepository;

        public InsertP1ConsumptionHandler(IP1ConsumptionRepository p1ConsumptionRepository)
        {
            _p1ConsumptionRepository = p1ConsumptionRepository;
        }

        public async Task<InsertP1ConsumptionResponse> Handle(InsertP1ConsumptionRequest request, CancellationToken cancellationToken)
        {
            var validator = new InsertP1ConsumptionRequestValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new HouseDBValidationException(result.ToString());
            }

            var existingDates = _p1ConsumptionRepository.GetExistingDates();

            var newItems = request.DomoticzP1Consumptions
                .Where(a_item => !existingDates.Contains(a_item.Date))
                .ToList();

            if (!newItems.Any())
            {
                return new InsertP1ConsumptionResponse();
            }

            foreach (var item in newItems)
            {
                // Skip today as the value will change
                if (item.Date.Date == DateTime.Today)
                {
                    continue;
                }

                _p1ConsumptionRepository.Add(new Entities.P1Consumption
                {
                    Date = item.Date,
                    DayUsage = item.DayUsage
                });
            }

            return new InsertP1ConsumptionResponse();
        }
    }
}
