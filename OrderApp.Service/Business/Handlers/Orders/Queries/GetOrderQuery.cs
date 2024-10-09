
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Microsoft.EntityFrameworkCore;


namespace Business.Handlers.Orders.Queries
{
    public class GetOrderQuery : IRequest<IDataResult<Order>>
    {
        public int Id { get; set; }

        public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, IDataResult<Order>>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;

            public GetOrderQueryHandler(IOrderRepository orderRepository, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Order>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
            {
                var order = await _orderRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false,
                    include: q => q.Include(s => s.Customer).ThenInclude(s => s.Address).Include(s => s.ProductOrders).ThenInclude(s => s.Product));
                return new SuccessDataResult<Order>(order);
            }
        }
    }
}
