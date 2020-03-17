using FluentValidation;

namespace HouseDB.Core.UseCases.P1Consumption
{
    public class P1WeekUsageRequestValidator : AbstractValidator<P1WeekUsageRequest>
    {
        public P1WeekUsageRequestValidator()
        {
            RuleFor(item => item.AmountOfWeeks).GreaterThan(0);
        }
    }
}
