using AutoMapper;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Basket;
using MyEccommerce.Route.Domain.Exceptions.BasketExceptions;
using MyEccommerce.Route.Services.Abstractions.Baskets;
using MyEccommerce.Route.Shared.Dtos.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Baskets
{
    public class BasketService(IBasketRepository _basketRepository,IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto?> GetUserBasketAsync(string basketId)
        {
            var basket=await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) throw new BasketNotFoundException(basketId);

            return _mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto?> UpdateUserBasketAsync(BasketDto basket)
        {
            var basketResult = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(basket), TimeSpan.FromDays(30));

            if(basketResult is null) throw new CreateOrUpdateBasketBadRequestException();

            return _mapper.Map<BasketDto>(basketResult);
        }
        public async Task<bool> DeleteUserBasketAsync(string id)
        {
            var Flag= await _basketRepository.DeleteBasketAsync(id);
            if (!Flag) throw new DeleteBasketBadRequestException();

            return Flag;
        }

        public async Task<BasketDto?> ClearBasketAsync(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket is null) throw new BasketNotFoundException(id);
            var emptyBasket = new CustomerBasket() { Id=id,Items = new List<BasketItem>() };
            var newBasket = await _basketRepository.UpdateBasketAsync(emptyBasket, TimeSpan.FromDays(30));

            if (newBasket is null) throw new CreateOrUpdateBasketBadRequestException();
            return _mapper.Map<BasketDto>(newBasket);
            ;

        }
    }
}
