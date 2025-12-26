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
    public class AdminOrderSpecification : BaseSpecification<Guid, Order>
    {
        public AdminOrderSpecification(AdminOrderQueryParameters parameters) : base(o=>
            (string.IsNullOrEmpty(parameters.Search)||o.userEmail.ToLower().Contains(parameters.Search.ToLower()))
            
            )
        {
            ApplySort(parameters.Sort);
            ApplyPagination(parameters.PageIndex, parameters.PageSize);
            ApplyInclude();
        }
        private void ApplyInclude()
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
        private void ApplySort(string? sort)
        {
            if (string.IsNullOrEmpty(sort))
            {
                ApplyOrderBy(o => o.OrderDate);
            }
            else
            {
                switch (sort.ToLower())
                {
                    case "email":
                        ApplyOrderBy(o => o.userEmail);
                        break;
                    case "priceasc":
                        ApplyOrderBy(o => o.Subtotal + o.DeliveryMethod.Cost);
                        break;
                    case "pricedesc":
                        ApplyOrderByDescendingly(o => o.Subtotal + o.DeliveryMethod.Cost);
                        break;
                    
                    default:
                        ApplyOrderBy(o => o.OrderDate);
                        break;
                }
            }
        }
    }
}
