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
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Products.Queries
{
    public class GetProductsWithPaginationQuery : IRequest<IDataResult<IEnumerable<Product>>>
    {
        public int Page { get; set; }
        public class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, IDataResult<IEnumerable<Product>>>
        {
            private readonly IProductRepository _productRepository;
            private readonly IMediator _mediator;

            public GetProductsWithPaginationQueryHandler(IProductRepository productRepository, IMediator mediator)
            {
                _productRepository = productRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Product>>> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Product>>( _productRepository
                    .GetListForPaging(request.Page,"CreatedDate",false,q=>q.isDeleted==false,q=>q.Category,q=>q.Color,q=>q.Storage).Data);


                    //.GetListAsync(p => p.isDeleted == false,
                    //include: q => q.Include(s => s.Color).Include(s => s.Category)
                    //.Include(s => s.Storage).ThenInclude(s => s.Address)));
            }
        }
    }
}
