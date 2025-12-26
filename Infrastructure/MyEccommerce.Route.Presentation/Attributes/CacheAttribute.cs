using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MyEccommerce.Route.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Attributes
{
    public class CacheAttribute(int durationInSec) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;
            var key = GenerateKey(context.HttpContext);
            var Cached = await CacheService.GetCacheAsync(key);
            if (!string.IsNullOrEmpty(Cached))
            {
                context.Result = new ContentResult() {
                    ContentType = "Application/json",
                    StatusCode = StatusCodes.Status200OK,
                    Content = Cached
                };
                return;
            }

            var Result = await next();
            if(Result.Result is OkObjectResult objectResult)
            {
                await CacheService.SetCacheAsync(key, objectResult, TimeSpan.FromSeconds(durationInSec));
            }
        } 
        private string GenerateKey(HttpContext context)
        {
            var key = new StringBuilder();
            key.Append(context.Request.Path);

            foreach (var param in context.Request.Query.OrderBy(o=>o.Key))
            {
                key.Append($"|{param.Key}-{param.Value}");
            }
            return key.ToString();
        }
    }
}
