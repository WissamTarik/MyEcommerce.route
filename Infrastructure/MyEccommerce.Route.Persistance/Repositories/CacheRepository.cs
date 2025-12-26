using MyEccommerce.Route.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer _connection) : ICacheRepository
    {
        private readonly  IDatabase _database =_connection.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
           var Result= await _database.StringGetAsync(key);
            if (Result.IsNullOrEmpty) return null;

            return Result;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
          var Flag=  await _database.StringSetAsync(key, JsonSerializer.Serialize(value), duration);
        
            
         }
    }
}
