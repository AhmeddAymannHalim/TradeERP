using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Commons;

namespace TradeERP.BLL.Validations.Commons
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserViewModel>
    {
        public UpdateUserValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x.SelectedRoles)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);
        }
    }
}
