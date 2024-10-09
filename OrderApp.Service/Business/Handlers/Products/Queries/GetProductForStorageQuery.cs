using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Products.Queries
{
    public class GetProductForStorageQuery : IRequest<IDataResult<IEnumerable<Product>>>
    {
        public int StorageId { get; set; }
        public class GetProductForStorageQueryHandler : IRequestHandler<GetProductForStorageQuery, IDataResult<IEnumerable<Product>>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;

            public GetProductForStorageQueryHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Product>>> Handle(GetProductForStorageQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Product>>(await _productRepository.GetListAsync(p => p.isDeleted == false && p.StorageId == request.StorageId,
                    include: q => q.Include(s => s.Color).Include(s => s.Category)
                    .Include(s => s.Storage).ThenInclude(s => s.Address)));
            }
        }
    }
}
