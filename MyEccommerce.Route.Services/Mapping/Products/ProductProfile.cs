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
    public class ProductProfile:Profile
    {
        public ProductProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(d=>d.Brand,s=>s.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d=>d.Type,s=>s.MapFrom(s=>s.ProductType.Name))
                .ForMember(d=>d.PictureUrl,s=>s.MapFrom(new PictureUrlResolver(configuration)));
            CreateMap<ProductBrand, ProductBrandAndTypeResponse>();
            CreateMap<ProductType, ProductBrandAndTypeResponse>();
            CreateMap<AddProductDto, Product>();
        }
    }
}
