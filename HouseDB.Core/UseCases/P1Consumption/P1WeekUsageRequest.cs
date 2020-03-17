using MediatR;

namespace HouseDB.Core.UseCases.P1Consumption
{
    public class P1WeekUsageRequest : IRequest<P1WeekUsageResponse>
    {
        public int AmountOfWeeks { get; set; }
    }
}
