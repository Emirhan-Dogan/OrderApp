using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework; // DbContext'inizi import edin
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccess.Concrete.EntityFramework.Contexts;
using Core.Aspects.Autofac.Transaction;

namespace Business.Handlers.Orders.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteOrderCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, IResult>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly MsDbContext _context;
            private readonly IMediator _mediator;

            public DeleteOrderCommandHandler(IOrderRepository orderRepository, MsDbContext context, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _context = context; 
                _mediator = mediator;
            }
            //[TransactionScopeAspect]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var orderToDelete = await _orderRepository.GetAsync(p => p.Id == request.Id,
                    include:q=>q.Include(s=>s.ProductOrders).ThenInclude(s=>s.Product));

                if (orderToDelete == null)
                {
                    return new ErrorResult(Messages.Unknown); 
                }

                var productOrders = orderToDelete.ProductOrders;

                foreach (var productOrder in productOrders)
                {
                    var product = await _context.Products
                                                .Where(p => p.Id == productOrder.ProductId)
                                                .FirstOrDefaultAsync(cancellationToken);

                    if (product != null)
                    {
                        product.StockCount += productOrder.Piece;
                        _context.Products.Update(product); 
                    }
                }

                orderToDelete.isDeleted = true;
                _orderRepository.Update(orderToDelete);
                await _context.SaveChangesAsync();

                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}
