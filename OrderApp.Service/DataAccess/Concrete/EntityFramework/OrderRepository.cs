
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using System.Collections.Generic;
using Entities.Dtos;
using System.Net.WebSockets;
namespace DataAccess.Concrete.EntityFramework
{
    public class OrderRepository : EfEntityRepositoryBase<Order, ProjectDbContext>, IOrderRepository
    {
        private ProjectDbContext _dbContext;
        public OrderRepository(ProjectDbContext context) : base(context)
        {
            this._dbContext = context;
        }

        public IEnumerable<LineChart> GetOrderSummaryByDate()
        {
            /*
             * SELECT 
                    CAST(DateOfReceipt AS DATE) AS OrderDate,
                    COUNT(*) AS TotalOrders,
                    SUM(TotalPrice) AS TotalSales
                FROM 
                    Orders
                GROUP BY 
                    CAST(DateOfReceipt AS DATE)
                ORDER BY 
                    OrderDate;
             */

            var result = (from o in _dbContext.Orders
                          group o by o.DateOfReceipt.Date into g
                          select new LineChart
                          {
                              Date = g.Key,
                              OrderCount = g.Count(),
                              TotalPrice = (float)g.Sum(o => o.TotalPrice)
                          })
                          .OrderBy(r => r.Date)
                          .ToList();

            return result;

        }
    }
}
