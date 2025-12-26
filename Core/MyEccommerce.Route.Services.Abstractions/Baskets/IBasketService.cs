using MyEccommerce.Route.Shared.Dtos.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Baskets
{
    public interface IBasketService
    {
         Task<BasketDto?> GetUserBasketAsync(string basketId);
         Task<BasketDto?> UpdateUserBasketAsync(BasketDto basket);
         Task<bool> DeleteUserBasketAsync(string id);
        Task<BasketDto?> ClearBasketAsync(string id);
    }
}
