using MyEccommerce.Route.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Specification
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Guid, Order>
    {
        public OrderWithPaymentIntentSpecification(string? paymentIntentId) : base(o=>o.PaymentIntentId==paymentIntentId)
        {
        }
    }
}
