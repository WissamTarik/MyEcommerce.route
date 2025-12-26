using MyEccommerce.Route.Services.Abstractions.Admin;
using MyEccommerce.Route.Services.Abstractions.Auth;
using MyEccommerce.Route.Services.Abstractions.Baskets;
using MyEccommerce.Route.Services.Abstractions.Notifications;
using MyEccommerce.Route.Services.Abstractions.Orders;
using MyEccommerce.Route.Services.Abstractions.Payments;
using MyEccommerce.Route.Services.Abstractions.Products;
using MyEccommerce.Route.Services.Abstractions.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get; }
        ICacheService CacheService { get; }
        IAuthService  AuthService { get; }
        IOrderService OrderService { get; }
        IPaymentService  PaymentService { get; }
        IRoleService RoleService { get; }
        IAdminService AdminService { get; }
    }
}
