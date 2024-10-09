
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Transaction;


namespace Business.Handlers.ProductOrders.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteProductOrderCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteProductOrderCommandHandler : IRequestHandler<DeleteProductOrderCommand, IResult>
        {
            private readonly IProductOrderRepository _productOrderRepository;
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;

            public DeleteProductOrderCommandHandler(IProductOrderRepository productOrderRepository, IProductRepository productRepository, IMediator mediator)
            {
                _productOrderRepository = productOrderRepository;
                _productRepository = productRepository;
                _mediator = mediator;
            }
            [TransactionScopeAspect]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteProductOrderCommand request, CancellationToken cancellationToken)
            {
                var productOrderToDelete = _productOrderRepository.Get(p => p.Id == request.Id);
                productOrderToDelete.isDeleted = true;

                var product = _productRepository.Get(p => p.Id == productOrderToDelete.ProductId);
                product.StockCount += productOrderToDelete.Piece;

                _productRepository.Update(product);
                _productRepository.SaveChanges();
                _productOrderRepository.Update(productOrderToDelete);
                await _productOrderRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

