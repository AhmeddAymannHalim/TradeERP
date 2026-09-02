using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.EntryDetails
{
    public class EntryDetailsValidator : AbstractValidator<EntryDetailsViewModel>
    {
        public EntryDetailsValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<DAL.Models.EntryDetails> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);

            RuleFor(x => x.EntryMasterId)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x.LedgerAccountId)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);
        }
    }
}
