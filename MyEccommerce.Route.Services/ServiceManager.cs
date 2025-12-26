using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Services.Abstractions.Admin;
using MyEccommerce.Route.Services.Abstractions.Auth;
using MyEccommerce.Route.Services.Abstractions.Baskets;
using MyEccommerce.Route.Services.Abstractions.Emails;
using MyEccommerce.Route.Services.Abstractions.Notifications;
using MyEccommerce.Route.Services.Abstractions.Orders;
using MyEccommerce.Route.Services.Abstractions.Payments;
using MyEccommerce.Route.Services.Abstractions.Products;
using MyEccommerce.Route.Services.Abstractions.Roles;
using MyEccommerce.Route.Services.Admin;
using MyEccommerce.Route.Services.Auth;
using MyEccommerce.Route.Services.Baskets;
using MyEccommerce.Route.Services.Orders;
using MyEccommerce.Route.Services.Payments;
using MyEccommerce.Route.Services.Products;
using MyEccommerce.Route.Shared.Jwt;
using MyEccommerce.Route.Shared.StripeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services
{
    public class ServiceManager(IUnitOfWok _unitOfWok,
                                 IBasketRepository _basketRepository,
                                 ICacheRepository cacheRepository,
                                 UserManager<AppUser> _userManager,
                                 RoleManager<IdentityRole> _roleManager,
                                 IOptions<JwtOptions> _options,
                                 IOptions<StripeOption> _stripeOptions,
                                 IEmailService _emailService,
                                 IImageService _imageService,
                                 INotificationService _notificationService,
                                 IConfiguration _configuration,
                                 IMapper _mapper) : IServiceManager
       
    {
        public IProductService ProductService { get; }=new ProductService(_unitOfWok,_mapper,_imageService);

        public IBasketService BasketService { get; }=new BasketService(_basketRepository,_mapper);

        public ICacheService CacheService { get; }=new CacheService(cacheRepository);

        public IAuthService AuthService { get; }=new AuthService(_userManager,_options,_mapper,_emailService,_configuration);

        public IOrderService OrderService { get; }=new OrderService(_unitOfWok,_mapper,_userManager,_basketRepository,_notificationService);

        public IPaymentService PaymentService { get; }=new PaymentService(_basketRepository,_unitOfWok,_mapper,_stripeOptions,_notificationService);

        public IRoleService RoleService { get; }=new RoleService(_userManager,_roleManager);

        public IAdminService AdminService { get; }=new AdminService(_userManager,_roleManager);
    }
}
