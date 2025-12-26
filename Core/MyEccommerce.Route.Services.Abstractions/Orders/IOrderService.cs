using MyEccommerce.Route.Shared;
using MyEccommerce.Route.Shared.Dtos.Orders;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Orders
{
    public interface IOrderService
    {
      Task<OrderResponse?>  CreateOrderAsync(OrderRequest orderRequest,string userEmail);

        Task<IEnumerable<DeliveryMethodResponse>> GetDeliveryMethods();
        Task<OrderResponse?> GetOrderByIdForUserAsync(Guid Id, string userEmail);
        Task<OrderResponse?> GetOrderByIdAsync(Guid Id);
        Task<IEnumerable<OrderResponse>?> GetOrdersForUserAsync( string userEmail);
        Task<AdminOrderStatusDto> GetAdminOrderStatusAsync();
        Task<PaginationResponse<AdminOrderListDto>> GetOrdersForAdminAsync(AdminOrderQueryParameters parameters);
    }
}
