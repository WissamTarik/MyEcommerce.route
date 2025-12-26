using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Domain.Entities.Orders;
using MyEccommerce.Route.Persistence.Identity;
using MyEcommerce.Route.Domain.Entities.Products;
using MyEcommerce.Route.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence
{
    public class DbInitializer(MyEcommerceDbContext _context,MyEcommerceIdentityDbContext _identityDbContext,UserManager<AppUser> _userManager,RoleManager<IdentityRole> _roleManager) : IDbInitializer
    {
        public async Task InitializeDbAsync()
        {
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
              await  _context.Database.MigrateAsync();
            }
            if (!_context.ProductBrands.Any())
            {
                var BrandsFile = await File.ReadAllTextAsync(@"..\Infrastructure\MyEccommerce.Route.Persistance\Data\DataSeeding\brands.json");
            
               if(!string.IsNullOrEmpty(BrandsFile))
                {
                    var Brands =  JsonSerializer.Deserialize<List<ProductBrand>>(BrandsFile); 

                    if(Brands is not null && Brands.Count()>0)
                    {
                        await  _context.AddRangeAsync(Brands);
                    }
                }
            }

            if (!_context.ProductTypes.Any())
            {
                var TypesFile = await File.ReadAllTextAsync(@"..\Infrastructure\MyEccommerce.Route.Persistance\Data\DataSeeding\types.json");
                if (!string.IsNullOrEmpty(TypesFile))
                {
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesFile);
                    if(Types is not null && Types.Count() > 0)
                    {
                        await _context.AddRangeAsync(Types);
                    }
                }
            }

            if (!_context.Products.Any())
            {
                var ProductFile = await File.ReadAllTextAsync(@"..\Infrastructure\MyEccommerce.Route.Persistance\Data\DataSeeding\products.json");
                if (!string.IsNullOrEmpty(ProductFile))
                {
                    var Product= JsonSerializer.Deserialize<List<Product>>(ProductFile);
                    if ( Product is not null&& Product.Count() > 0)
                    {
                            await _context.AddRangeAsync(Product);
                    }
                }
            
            }
            if (!_context.DeliveryMethods.Any())
            {
                var DeliveryMethodFile = await File.ReadAllTextAsync(@"..\Infrastructure\MyEccommerce.Route.Persistance\Data\DataSeeding\delivery.json");

                if(DeliveryMethodFile is not null)
                {
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodFile);
                    if(DeliveryMethods is not null && DeliveryMethods.Count() > 0)
                    {
                       await _context.AddRangeAsync(DeliveryMethods);
                    }
                }
            }


           await _context.SaveChangesAsync();
        }

        public async Task InitializeIdentityDbAsync()
        {
            if (_identityDbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
               await  _identityDbContext.Database.MigrateAsync();
            }
            if  (!_identityDbContext.Roles.Any())
            {
                var Admin = new IdentityRole() { Name = "Admin" };
                var SuperAdmin = new IdentityRole() { Name = "SuperAdmin" };
                await _roleManager.CreateAsync(Admin);
                await _roleManager.CreateAsync(SuperAdmin);
               
            }

            if (!_identityDbContext.Users.Any())
            {

                var SuperAdminUser = new AppUser()
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789"
                };
                var AdminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789"
                };
             await   _userManager.CreateAsync(SuperAdminUser, "P@ssW0rd");
             await   _userManager.CreateAsync(AdminUser, "P@ssW0rd");
              await  _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(AdminUser, "Admin");
            }
              await  _identityDbContext.SaveChangesAsync();
        }
    }
}
