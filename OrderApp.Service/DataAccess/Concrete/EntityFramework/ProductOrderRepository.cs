
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
using System.Collections.Generic;
using Entities.Dtos;
namespace DataAccess.Concrete.EntityFramework
{
    public class ProductOrderRepository : EfEntityRepositoryBase<ProductOrder, ProjectDbContext>, IProductOrderRepository
    {
        private ProjectDbContext _dbContext;
        public ProductOrderRepository(ProjectDbContext context) : base(context)
        {
            this._dbContext = context;
        }

        public IEnumerable<DonutChart> GetProductOrderSummaryByCategories()
        {
            var result = from productOrders in _dbContext.ProductOrders
                         join products in _dbContext.Products on productOrders.ProductId equals products.Id
                         join categories in _dbContext.Categories on products.CategoryId equals categories.Id
                         group productOrders by categories.Name into g
                         select new DonutChart
                         {
                             Category = g.Key,
                             Value = g.Sum(po => po.Piece)
                         };

            return result.ToList();

        }
    }
}
