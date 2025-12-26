using MyEccommerce.Route.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Specification
{
    public class OrderSpecification : BaseSpecification<Guid, Order>
    {
        public OrderSpecification(string userEmail,Guid orderId) : base(o=>o.userEmail.ToLower()==userEmail.ToLower()&& o.Id==orderId)
        {
            ApplyInclude();
        }
        public OrderSpecification(string userEmail) : base(o=>o.userEmail.ToLower()==userEmail.ToLower())
        {
            ApplyInclude();
        }
        public OrderSpecification(Guid Id) : base(o=>o.Id==Id)
        {
            ApplyInclude();
        }
        public OrderSpecification() : base(null)
        {
            ApplyInclude();
        }
       
        private void ApplyInclude()
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
