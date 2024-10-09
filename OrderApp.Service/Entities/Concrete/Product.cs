using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public enum SizeCode
    {
        S,
        M,
        L,
        XL
    }
    public class Product : BaseEntity, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double Price { get; set; }
        public SizeCode SizeCode { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int StockCount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int StorageId { get; set; }
        public Storage Storage { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; }
    }

}
