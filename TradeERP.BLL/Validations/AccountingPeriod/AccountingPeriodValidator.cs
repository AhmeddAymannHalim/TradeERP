using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.AccountingPeriod
{
    public class AccountingPeriodValidator : AbstractValidator<AccountingPeriodViewModel>
    {
        public AccountingPeriodValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage(localizer["Val.RequiredField"]);
        }
    }
}
