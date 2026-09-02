using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.BillDetails
{
    public class BillDetailsValidator : AbstractValidator<BillDetailsViewModel>
    {
        public BillDetailsValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<DAL.Models.BillDetails> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);

            RuleFor(x => x.BillMasterId)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);
        }
    }
}
