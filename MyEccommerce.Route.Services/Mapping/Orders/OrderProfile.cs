using AutoMapper;
using MyEccommerce.Route.Domain.Entities.Orders;
using MyEccommerce.Route.Shared.Dtos.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Mapping.Orders
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<DeliveryMethod, DeliveryMethodResponse>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d=>d.ProductId,s=>s.MapFrom(s=>s.Product.ProductId))
                .ForMember(d=>d.ProductName,s=>s.MapFrom(s=>s.Product.ProductName))
                .ForMember(d=>d.PictureUrl,s=>s.MapFrom(s=>s.Product.PictureUrl));
            CreateMap<OrderAddress, OrderAddressDto>().ReverseMap();
            CreateMap<Order, OrderResponse>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d=>d.OrderAddress,o=>o.MapFrom(s=>s.ShippingAddress))
                .ForMember(d=>d.Total,o=>o.MapFrom(s=>s.GetTotal()))
                .ForMember(d=>d.OrderStatus,o=>o.MapFrom(s=>s.Status.ToString()));

        }
    }
}
