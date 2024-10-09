
using System;
using System.Collections;
using System.Collections.Generic;
using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
namespace DataAccess.Abstract
{
    public interface IProductOrderRepository : IEntityRepository<ProductOrder>
    {
        IEnumerable<DonutChart> GetProductOrderSummaryByCategories();
    }
}