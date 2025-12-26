using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyEccommerce.Route.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence.Identity
{
    public class MyEcommerceIdentityDbContext:IdentityDbContext<AppUser>
    {
        public MyEcommerceIdentityDbContext(DbContextOptions<MyEcommerceIdentityDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().ToTable("Addresses");
            builder.Ignore < IdentityUserClaim<string>>();
            builder.Ignore < IdentityRoleClaim<string>>();
            builder.Ignore < IdentityUserToken<string>>();
        }
    }
}
