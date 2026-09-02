using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.Validations.Commons
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordViewModel>
    {
        public ForgotPasswordValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .EmailAddress()
                .WithMessage(localizer["Val.InvalidEmail"]);
        }
    }
}
