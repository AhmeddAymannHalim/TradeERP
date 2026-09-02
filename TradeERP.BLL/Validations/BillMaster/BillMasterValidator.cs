using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.BillMaster
{
    public class BillMasterValidator : AbstractValidator<BillMasterViewModel>
    {
        public BillMasterValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<DAL.Models.BillMaster> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);
        }
    }
}
