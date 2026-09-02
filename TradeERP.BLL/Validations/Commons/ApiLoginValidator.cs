using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.Validations.Commons
{
    public class ApiLoginValidator : AbstractValidator<ApiLoginViewModel>
    {
        public ApiLoginValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .EmailAddress()
                .WithMessage(localizer["Val.InvalidEmail"]);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);
        }
    }
}
