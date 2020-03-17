using FluentValidation;

namespace HouseDB.Core.UseCases.P1Consumption
{
    public class P1MonthUsageRequestValidator : AbstractValidator<P1MonthUsageRequest>
    {
        public P1MonthUsageRequestValidator()
        {
            RuleFor(item => item.AmountOfMonths).GreaterThan(0);
        }
    }
}
