using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.EntryMaster
{
    public class EntryMasterValidator : AbstractValidator<EntryMasterViewModel>
    {
        public EntryMasterValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<TradeERP.DAL.Models.EntryMaster> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);
        }
    }
}
