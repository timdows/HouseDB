using FluentValidation;

namespace HouseDB.Core.UseCases.DomoticzCache
{
    public class InsertDomoticzDeviceValuesForCachingRequestValidator : AbstractValidator<InsertDomoticzDeviceValuesForCachingRequest>
    {
        public InsertDomoticzDeviceValuesForCachingRequestValidator()
        {
            RuleFor(item => item).NotNull();
        }
    }
}
