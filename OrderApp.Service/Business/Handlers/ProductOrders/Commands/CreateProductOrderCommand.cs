using Core.Aspects.Autofac.Transaction;
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.ProductOrders.ValidationRules;

namespace Business.Handlers.ProductOrders.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateProductOrderCommand : IRequest<IResult>
    {
        public int CreatedUserId { get; set; }
        public bool Status { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Piece { get; set; }
        public double UnitInPrice { get; set; }


        public class CreateProductOrderCommandHandler : IRequestHandler<CreateProductOrderCommand, IResult>
        {
            private readonly IProductOrderRepository _productOrderRepository;
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;
            public CreateProductOrderCommandHandler(IProductOrderRepository productOrderRepository, IProductRepository productRepository, IMediator mediator)
            {
                _productOrderRepository = productOrderRepository;
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [TransactionScopeAspect]
            [ValidationAspect(typeof(CreateProductOrderValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateProductOrderCommand request, CancellationToken cancellationToken)
            {
                //var isThereProductOrderRecord = _productOrderRepository.Query().Any(u => u.CreatedDate == request.CreatedDate);

                //if (isThereProductOrderRecord == true)
                //    return new ErrorResult(Messages.NameAlreadyExist);
                var product = _productRepository.Get(O => O.Id == request.ProductId);
                if (request.Piece > product.StockCount)
                {
                    return new ErrorResult(Messages.StorckError);
                }
                product.StockCount = product.StockCount - request.Piece;

                
                var addedProductOrder = new ProductOrder
                {
                    CreatedDate = System.DateTime.Now,
                    LastUpdatedUserId = request.CreatedUserId,
                    LastUpdatedDate = System.DateTime.Now,
                    Status = request.Status,
                    isDeleted = false,
                    CreatedUserId = request.CreatedUserId,
                    ProductId = request.ProductId,
                    OrderId = request.OrderId,
                    Piece = request.Piece,
                    UnitInPrice = request.UnitInPrice,

                };
                _productRepository.Update(product);
                _productRepository.SaveChanges();
                _productOrderRepository.Add(addedProductOrder);
                await _productOrderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}