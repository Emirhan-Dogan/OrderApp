using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Customer : BaseEntity, IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CustomerCode { get; set; }
        public int AddressId { get; set; }

        public Address Address { get; set; }
        public User User { get; set; }
    }
}
