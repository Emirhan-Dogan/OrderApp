
using Business.Handlers.Cities.Commands;
using FluentValidation;

namespace Business.Handlers.Cities.ValidationRules
{

    public class CreateCityValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();

        }
    }
    public class UpdateCityValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();

        }
    }
}