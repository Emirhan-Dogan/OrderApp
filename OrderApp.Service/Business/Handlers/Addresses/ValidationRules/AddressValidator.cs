
using Business.Handlers.Addresses.Commands;
using FluentValidation;

namespace Business.Handlers.Addresses.ValidationRules
{

    public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.AddressDetail).NotEmpty();
            RuleFor(x => x.DateOfReceipt).NotEmpty();

        }
    }
    public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.AddressDetail).NotEmpty();
            RuleFor(x => x.DateOfReceipt).NotEmpty();

        }
    }
}