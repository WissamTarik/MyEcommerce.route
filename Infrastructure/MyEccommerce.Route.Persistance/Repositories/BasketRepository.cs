using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Basket;
using MyEccommerce.Route.Domain.Exceptions.BasketExceptions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer _connection) : IBasketRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();
        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
          var RedisValue=  await   _database.StringGetAsync(id);
            if (RedisValue.IsNullOrEmpty) return null;
            var Result = JsonSerializer.Deserialize<CustomerBasket>(RedisValue);
            return Result;
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var Flag = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30)) ;

            return Flag ? await GetBasketAsync(basket.Id) : null;
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
           var Flag=   await  _database.KeyDeleteAsync(id);
            return Flag;
        }

      
    }
}
