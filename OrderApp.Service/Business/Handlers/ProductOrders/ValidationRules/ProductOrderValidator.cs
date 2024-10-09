
using Business.Handlers.ProductOrders.Commands;
using FluentValidation;

namespace Business.Handlers.ProductOrders.ValidationRules
{

    public class CreateProductOrderValidator : AbstractValidator<CreateProductOrderCommand>
    {
        public CreateProductOrderValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Piece).NotEmpty();
            RuleFor(x => x.UnitInPrice).NotEmpty();

        }
    }
    public class UpdateProductOrderValidator : AbstractValidator<UpdateProductOrderCommand>
    {
        public UpdateProductOrderValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Piece).NotEmpty();
            RuleFor(x => x.UnitInPrice).NotEmpty();

        }
    }
}