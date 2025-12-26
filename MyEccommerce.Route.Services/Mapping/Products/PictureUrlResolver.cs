using AutoMapper;
using Microsoft.Extensions.Configuration;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEcommerce.Route.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Mapping.Products
{
    public class PictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductResponse, string>
    {
        public string Resolve(Product source, ProductResponse destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl)) return string.Empty;


            return $"{configuration["Localhost"]}/{source.PictureUrl}";
        }
    }
}
