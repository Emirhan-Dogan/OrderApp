using Business.BusinessAspects;
using Castle.DynamicProxy.Generators.Emitters;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Dashboard.Queries
{
    public class GetDashboardDataQuery : IRequest<IDataResult<DashboardDataDTO>>
    {
        public int Id { get; set; }

        public class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, IDataResult<DashboardDataDTO>>
        {
            private readonly MsDbContext _context;
            private readonly IOrderRepository _orderRepository;
            private readonly IProductOrderRepository _productOrderRepository;

            public GetDashboardDataQueryHandler(MsDbContext context, IOrderRepository orderRepository, IProductOrderRepository productOrderRepository)
            {
                _context = context;
                _orderRepository = orderRepository;
                _productOrderRepository = productOrderRepository;
            }

            [SecuredOperation(Priority = 1)]
            [LogAspect(typeof(FileLogger))]
            public async Task<IDataResult<DashboardDataDTO>> Handle(GetDashboardDataQuery request, CancellationToken cancellationToken)
            {
                var lineChartData = _orderRepository.GetOrderSummaryByDate();
                var donutChartData = _productOrderRepository.GetProductOrderSummaryByCategories();
                var dashboardData = new DashboardDataDTO
                {
                    TotalOrders = await _context.Orders.Where(p => p.isDeleted == false).CountAsync(),
                    TotalCustomers = await _context.Customers.Where(p => p.isDeleted == false).CountAsync(),
                    TotalStorages = await _context.Storages.Where(p => p.isDeleted == false).CountAsync(),
                    TotalEmployees = await _context.Users.Where(p => p.isDeleted == false).CountAsync(),
                    TotalProducts = await _context.Products.Where(p => p.isDeleted == false).CountAsync(),
                    LineChartData = lineChartData,
                    DonutChartData = donutChartData
                };
                dashboardData.TotalEmployees -= dashboardData.TotalCustomers;
                return new SuccessDataResult<DashboardDataDTO>(dashboardData);
            }
        }
    }
}
