using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions
{
    public interface ICacheService
    {
        Task<string?> GetCacheAsync(string key);
        Task SetCacheAsync(string key,object value,TimeSpan duration);
    }
}
