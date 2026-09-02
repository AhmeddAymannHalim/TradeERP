using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.BillSetting
{
    public class BillSettingValidator : AbstractValidator<BillSettingViewModel>
    {
        public BillSettingValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<DAL.Models.BillSetting> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);
        }
    }
}
