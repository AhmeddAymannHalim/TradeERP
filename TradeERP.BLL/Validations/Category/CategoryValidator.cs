using FluentValidation;
using Microsoft.Extensions.Localization;
using TradeERP.BLL.IServices.ICommons;
using TradeERP.Shared;
using TradeERP.Shared.ViewModels.Definitions;

namespace TradeERP.BLL.Validations.Category
{
    public class CategoryValidator : AbstractValidator<CategoryViewModel>
    {
        public CategoryValidator(
            IStringLocalizer<SharedResource> localizer,
            IValidatorService<DAL.Models.Category> validator)
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
