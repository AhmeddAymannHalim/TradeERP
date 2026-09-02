using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.EntrySetting
{
    public class EntrySettingValidator : AbstractValidator<EntrySettingViewModel>
    {
        public EntrySettingValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<TradeERP.DAL.Models.EntrySetting> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);
        }
    }
}
