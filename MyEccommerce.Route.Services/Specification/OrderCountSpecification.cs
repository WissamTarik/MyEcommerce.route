using MyEccommerce.Route.Domain.Entities.Orders;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Specification
{
    public class OrderCountSpecification : BaseSpecification<Guid, Order>
    {
        public OrderCountSpecification(AdminOrderQueryParameters parameters) : base(o=>
            (string.IsNullOrEmpty(parameters.Search)||o.userEmail.ToLower()
            .Contains(parameters.Search.ToLower())))
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
