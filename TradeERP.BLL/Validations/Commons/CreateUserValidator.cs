using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.Validations.Commons
{
    public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .EmailAddress()
                .WithMessage(localizer["Val.InvalidEmail"]);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .MinimumLength(6)
                .WithMessage(localizer["Val.PasswordMinLength"]);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Equal(x => x.Password)
                .WithMessage(localizer["Val.PasswordsDoNotMatch"]);

            RuleFor(x => x.SelectedRoles)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);
        }
    }
}
