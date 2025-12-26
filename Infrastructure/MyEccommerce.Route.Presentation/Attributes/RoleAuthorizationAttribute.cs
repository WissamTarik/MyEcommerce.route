using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MyEccommerce.Route.Domain.Exceptions.Auth;
using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using MyEccommerce.Route.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Attributes
{
    public class RoleAuthorizationAttribute( params string[] roles) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            if (user?.Identity is null || !user.Identity.IsAuthenticated)
                throw new UnAuthorizedException("You must login first");

            var userEmail = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? throw new UnAuthorizedException("Invalid token");

            var RoleService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().RoleService;

            var userRoles = await RoleService.GetUserRolesAsync(userEmail);

            var IsAuthorized= userRoles.Any((role) => roles.Contains(role, StringComparer.OrdinalIgnoreCase));

            if (!IsAuthorized) throw new UnAuthorizedException("You don't have the permission to make this operation");

            await next();
        
        }
    }
}
