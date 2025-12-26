using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Domain.Entities.Orders;
using MyEccommerce.Route.Domain.Exceptions.Auth;
using MyEccommerce.Route.Domain.Exceptions.BasketExceptions;
using MyEccommerce.Route.Domain.Exceptions.Orders;
using MyEccommerce.Route.Domain.Exceptions.ProductExceptions;
using MyEccommerce.Route.Services.Abstractions.Notifications;
using MyEccommerce.Route.Services.Abstractions.Orders;
using MyEccommerce.Route.Services.Specification;
using MyEccommerce.Route.Shared.Dtos.Orders;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using MyEcommerce.Route.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Orders
{
    public class OrderService(IUnitOfWok _unitOfWok,IMapper _mapper,UserManager<AppUser> _userManager,IBasketRepository _basketRepository,INotificationService _notificationService) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            var basket = await _basketRepository.GetBasketAsync(orderRequest.BasketId);
            if (basket is null) throw new BasketNotFoundException(orderRequest.BasketId);
            var shippingAddress = _mapper.Map<OrderAddress>(orderRequest.ShippingAddress);
            var deliveryMethod = await _unitOfWok.GetRepository<int, DeliveryMethod>().GetByIdAsync(orderRequest.DeliveryMethodId);
            if(deliveryMethod is null) throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);


            var orderItems = new List<OrderItem>();
            ArgumentException.ThrowIfNullOrEmpty(basket.PaymentIntentId);
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWok.GetRepository<int, Product>().GetByIdAsync(item.Id);
                if (product is null) throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInOrderItem,item.Quantity, item.Price);
                orderItems.Add(orderItem);
            }
            var subTotal=orderItems.Sum(o=>o.Price*o.Quantity);
            var orderRepo = _unitOfWok.GetRepository<Guid, Order>();
            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var ExistOrder = await orderRepo.GetByIdAsync(spec);
            if (ExistOrder is not null) orderRepo.Delete(ExistOrder);
                var order=new Order(userEmail, shippingAddress,deliveryMethod,orderItems,subTotal,basket.PaymentIntentId);
            await orderRepo.AddAsync(order);
            await _unitOfWok.SaveChangeAsync();
            await _notificationService.SendNotificationAsync(order.userEmail, "Order created",
                                           $"Your order {order.Id} has been created successfully");
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethods()
        {
            var Result=await _unitOfWok.GetRepository<int, DeliveryMethod>().GetAllAsync();

            if (Result is null) throw new DeliveryMethodBadRequestException();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(Result);
        }

        public async Task<OrderResponse?> GetOrderByIdForUserAsync(Guid Id, string userEmail)
        {
           var user=await  _userManager.FindByEmailAsync(userEmail);
            if (user is null) throw new UserNotFoundException("email",userEmail);
            var orderSpecification = new OrderSpecification(userEmail, Id);
              var Order=   await   _unitOfWok.GetRepository<Guid, Order>().GetByIdAsync(orderSpecification);
            if (Order is null) throw new OrderNotFoundException(Id);
            return _mapper.Map<OrderResponse>(Order);
        }

        public async Task<IEnumerable<OrderResponse>?> GetOrdersForUserAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null) throw new UserNotFoundException("email",userEmail);
            var orderSpecification = new OrderSpecification(userEmail);
            var orders=await   _unitOfWok.GetRepository<Guid,Order>().GetAllAsync(orderSpecification);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
            
        }

        public async Task<int> GetOrdersCountAsync()
        {
            var Orders = await _unitOfWok.GetRepository<Guid, Order>().GetAllAsync();
            return Orders.Count();
        }
        public async Task<AdminOrderStatusDto> GetAdminOrderStatusAsync()
        {
            var spec = new OrderSpecification();
          
            var ordersRepo =  _unitOfWok.GetRepository<Guid, Order>();
            var orders = await ordersRepo.GetAllAsync(spec);
            var ordersPending =orders.Count(o=>o.Status==OrderStatus.Pending);
            var ordersSuccess = orders.Count(o => o.Status == OrderStatus.PaymentSuccess);
            var ordersFailed = orders.Count(o => o.Status == OrderStatus.PaymentFailed);
            var avg = await ordersRepo.AverageAsync(spec, o => o.Subtotal + o.DeliveryMethod.Cost);
            return new AdminOrderStatusDto()
            {
                OrdersAverage =avg,
                TotalOrders = orders.Count(),
                TotalRevenue=orders.Sum(o=>o.Subtotal+o.DeliveryMethod?.Cost??0m),
                TotalPendingOrders=ordersPending,
                TotalSuccessOrders=ordersSuccess,
                TotalFailedOrders=ordersFailed,
            };
        }

        public async Task<PaginationResponse<AdminOrderListDto>> GetOrdersForAdminAsync(AdminOrderQueryParameters parameters)
        {
            var spec = new AdminOrderSpecification(parameters);
            var countSpec = new OrderCountSpecification(parameters);

            var OrderRepo = _unitOfWok.GetRepository<Guid, Order>();
            var orders = await OrderRepo.GetAllAsync(spec);
            var count = await OrderRepo.CountAsync(countSpec);

            List<AdminOrderListDto> adminOrderLists = orders.Select((order) => new AdminOrderListDto()
            {
                OrderId = order.Id,
                CustomerEmail=order.userEmail,
                Status=order.Status.ToString(),
                DeliveryMethod=order.DeliveryMethod.ShortName,
                OrderDate=order.OrderDate,
                PaymentIntentId=order.PaymentIntentId,
                TotalPrice=order.GetTotal()
            }).ToList();
            var paginationResponse = new PaginationResponse<AdminOrderListDto>
                   (count, parameters.PageSize, parameters.PageIndex, adminOrderLists);
            
            return paginationResponse;
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(Guid Id)
        {
            var spec = new OrderSpecification(Id);
            var OrderRepo = _unitOfWok.GetRepository<Guid, Order>();
            var order=await OrderRepo.GetByIdAsync(spec)?? throw new OrderNotFoundException(Id);

            return _mapper.Map<OrderResponse>(order);
        }
    }
}
