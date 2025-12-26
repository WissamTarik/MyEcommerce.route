using MyEccommerce.Route.Shared.QueryParameters;
using MyEcommerce.Route.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Specification
{
    public class ProductSpecification:BaseSpecification<int,Product>
    {
        public ProductSpecification(ProductQueryParameters parameters) :base(p=>
             (!parameters.BrandId.HasValue||p.BrandId==parameters.BrandId )&&
             (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId)&&
             (!parameters.MinPrice.HasValue || p.Price>= parameters.MinPrice)&&
             (!parameters.MaxPrice.HasValue || p.Price<= parameters.MaxPrice)&&
             (string.IsNullOrEmpty(parameters.Search)|| p.Name.ToLower().Contains(parameters.Search.ToLower()))
             
            )
        {
            ApplyOrder(parameters.Sort);
            ApplyPagination(parameters.PageIndex, parameters.PageSize);
            ApplyInclude();
        }
        public ProductSpecification(int id):base(p=>p.Id==id)
        {
            ApplyInclude();   
        }
        private void ApplyInclude()
        {
            Includes.Add(p=>p.ProductBrand);
            Includes.Add(p=>p.ProductType);
        }
        private void ApplyOrder(string? sort)
        {
            if (string.IsNullOrEmpty(sort)) ApplyOrderBy(p => p.Name);
            else
            {


                switch (sort)
                {
                    case "priceasc":
                        ApplyOrderBy(p => p.Price);
                        break;

                    case "pricedesc":
                        ApplyOrderByDescendingly(p => p.Price);
                        break;

                    default:
                        ApplyOrderBy(p => p.Name);
                        break;
                }
            }
        }
    }
}
