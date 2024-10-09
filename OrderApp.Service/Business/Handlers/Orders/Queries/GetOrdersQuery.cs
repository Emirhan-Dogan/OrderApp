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
using Newtonsoft.Json;

namespace Business.Handlers.Orders.Queries
{
    public class GetOrdersQuery : IRequest<IDataResult<IEnumerable<Order>>>
    {
        

        public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IDataResult<IEnumerable<Order>>>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMediator _mediator;

            // JSON serileştirme ayarları
            private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            public GetOrdersQueryHandler(IOrderRepository orderRepository, IMediator mediator)
            {
                _orderRepository = orderRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Order>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
            {
                // Verileri al
                var orders = await _orderRepository.GetListAsync(
                    p => p.isDeleted == false,
                    include: q => q
                        .Include(o => o.Customer)
                        .ThenInclude(c => c.User)
                        .Include(o => o.ProductOrders)
                        .ThenInclude(po => po.Product));

                // JSON serileştirme ile döngüsel referansları önleyin
                var jsonData = JsonConvert.SerializeObject(orders, _serializerSettings);

                // JSON veriyi tekrar deserialize ederek IEnumerable<Order> türüne çevirin
                var resultOrders = JsonConvert.DeserializeObject<IEnumerable<Order>>(jsonData);

                return new SuccessDataResult<IEnumerable<Order>>(resultOrders);
            }
        }
    }
}
