
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


namespace Business.Handlers.ProductOrders.Queries
{
    public class GetProductOrderQuery : IRequest<IDataResult<ProductOrder>>
    {
        public int Id { get; set; }

        public class GetProductOrderQueryHandler : IRequestHandler<GetProductOrderQuery, IDataResult<ProductOrder>>
        {
            private readonly IProductOrderRepository _productOrderRepository;
            private readonly IMediator _mediator;

            public GetProductOrderQueryHandler(IProductOrderRepository productOrderRepository, IMediator mediator)
            {
                _productOrderRepository = productOrderRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<ProductOrder>> Handle(GetProductOrderQuery request, CancellationToken cancellationToken)
            {
                var productOrder = await _productOrderRepository.GetAsync(p => p.Id == request.Id && p.isDeleted == false,
                    include: q => q.Include(s => s.Product).ThenInclude(s => s.Storage).Include(s => s.Product).ThenInclude(s => s.Category)
                    );
                return new SuccessDataResult<ProductOrder>(productOrder);
            }
        }
    }
}
