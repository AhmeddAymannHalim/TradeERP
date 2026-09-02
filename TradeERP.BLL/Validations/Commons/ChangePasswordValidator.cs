using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.Validations.Commons
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .MinimumLength(6)
                .WithMessage(localizer["Val.PasswordMinLength"]);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Equal(x => x.NewPassword)
                .WithMessage(localizer["Val.PasswordsDoNotMatch"]);
        }
    }
}
