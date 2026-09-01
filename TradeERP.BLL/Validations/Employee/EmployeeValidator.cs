using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.Employee
{
    public class EmployeeValidator : AbstractValidator<EmployeeViewModel>
    {
        public EmployeeValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<TradeERP.DAL.Models.Employee> validator)
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

            // Client sends the number as full E.164 (e.g. "+201012345678") via intl-tel-input's
            // country-code dropdown - this just re-checks the shape server-side too.
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"])
                .Matches(@"^\+[1-9]\d{6,14}$")
                .WithMessage(localizer["Val.InvalidPhoneNumber"]);

            RuleFor(x => x.NationalId)
                .NotEmpty()
                .WithMessage(localizer["Val.RequiredField"]);
        }
    }
}
