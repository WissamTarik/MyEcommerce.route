using MyEccommerce.Route.Shared.QueryParameters;
using MyEcommerce.Route.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Specification
{
    public class ProductCountSpecification:BaseSpecification<int,Product>
    {
        public ProductCountSpecification(ProductQueryParameters parameters):base(p=>
             (!parameters.BrandId.HasValue||p.BrandId==parameters.BrandId )&&
             (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId)&&
            (!parameters.MinPrice.HasValue || p.Price >= parameters.MinPrice) &&
             (!parameters.MaxPrice.HasValue || p.Price <= parameters.MaxPrice) &&
             (string.IsNullOrEmpty(parameters.Search)|| p.Name.ToLower().Contains(parameters.Search.ToLower()))
            
            )
        {
            
        }
    }
}
