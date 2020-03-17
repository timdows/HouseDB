using MediatR;

namespace HouseDB.Core.UseCases.P1Consumption
{
    public class P1MonthUsageRequest : IRequest<P1MonthUsageResponse>
    {
        public int AmountOfMonths { get; set; }
    }
}
