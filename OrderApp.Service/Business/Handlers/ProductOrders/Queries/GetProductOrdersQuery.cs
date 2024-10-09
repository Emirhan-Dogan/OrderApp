
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.ProductOrders.Queries
{

    public class GetProductOrdersQuery : IRequest<IDataResult<IEnumerable<ProductOrder>>>
    {
        public class GetProductOrdersQueryHandler : IRequestHandler<GetProductOrdersQuery, IDataResult<IEnumerable<ProductOrder>>>
        {
            private readonly IProductOrderRepository _productOrderRepository;
            private readonly IMediator _mediator;

            public GetProductOrdersQueryHandler(IProductOrderRepository productOrderRepository, IMediator mediator)
            {
                _productOrderRepository = productOrderRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<ProductOrder>>> Handle(GetProductOrdersQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<ProductOrder>>(await _productOrderRepository.GetListAsync(p=>p.isDeleted==false,
                    include: q=>q.Include(s => s.Product).ThenInclude(s => s.Storage).Include(s => s.Product).ThenInclude(s => s.Category)));
            }
        }
    }
}