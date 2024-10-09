
using Business.Handlers.Countries.Commands;
using FluentValidation;

namespace Business.Handlers.Countries.ValidationRules
{

    public class CreateCountryValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
    public class UpdateCountryValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();

        }
    }
}