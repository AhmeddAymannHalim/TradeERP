using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.Enums;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.VoucherMaster
{
    public class VoucherMasterValidator : AbstractValidator<VoucherMasterViewModel>
    {
        public VoucherMasterValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<DAL.Models.VoucherMaster> validator)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Must((model, code) => !validator.IsCodeExist(model.Id, code))
                .WithMessage(localizer["Val.CodeAlreadyExist"]);

            RuleFor(x => x.VoucherType)
                .IsInEnum()
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x.TreasuryLedgerAccountId)
                .GreaterThan(0)
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage(localizer["Val.RequiredField"]);

            RuleFor(x => x)
                .Must(x => x.VoucherType == VoucherType.Receipt ? x.CustomerId.HasValue : x.SupplierId.HasValue)
                .WithMessage(localizer["Val.RequiredField"])
                .OverridePropertyName(nameof(VoucherMasterViewModel.CustomerId));
        }
    }
}
