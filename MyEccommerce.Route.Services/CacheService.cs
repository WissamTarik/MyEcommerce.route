using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services
{
    public class CacheService(ICacheRepository _cacheRepository) : ICacheService
    {
        public async Task<string?> GetCacheAsync(string key)
        {
         var Result=  await  _cacheRepository.GetAsync(key);
            return string.IsNullOrEmpty(Result) ?default:Result;
        }

        public async Task SetCacheAsync(string key, object value, TimeSpan duration)
        {
            await _cacheRepository.SetAsync(key,value, duration);
        }
    }
}
