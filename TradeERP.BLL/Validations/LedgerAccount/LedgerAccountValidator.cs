using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.LedgerAccount
{
    public class LedgerAccountValidator : AbstractValidator<LedgerAccountViewModel>
    {
        public LedgerAccountValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<TradeERP.DAL.Models.LedgerAccount> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);

            RuleFor(x => x.ArName)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, arName) => !validator.IsArNameExist(model.Id, arName))
                .WithMessage(localizer["Val.NameAlreadyExist"]);

            RuleFor(x => x.EnName)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, enName) => !validator.IsEnNameExist(model.Id, enName))
                .WithMessage(localizer["Val.NameAlreadyExist"]);
        }
    }
}
