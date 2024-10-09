using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public enum DeliveryStatus
    {
        Preparing,   //Hazırlanıyor
        Shipped,     //Gönderildi
        OnWay,       //Yolda
        Delivered,   //Teslim edildi,
        Undelivered, //Teslim edilmedi
        Cancelled,   //İptal edildi
        Returned     //İade edildi
    }

    public class Order : BaseEntity, IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public double TotalPrice { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public string DeliveryNotes { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; }
    }

}
