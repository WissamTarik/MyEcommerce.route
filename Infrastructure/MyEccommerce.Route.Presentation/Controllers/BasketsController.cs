using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Shared.Dtos.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController(IServiceManager _serviceManager):ControllerBase
    {
        /// <summary>
        /// Get user basket by id
        /// </summary>
        /// <param name="id">The unique identifier of basket</param>
        /// <remarks>
        ///  Errors:
        ///  
        ///  -404:Product Not found
        ///  
        ///  -500:Invalid Id
        /// </remarks>
        ///<returns>Basket details</returns>
        /// <response code="200">Basket returned successfully</response>
        /// <response code="404">Basket not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasketById(string id)
        {
          var Result= await  _serviceManager.BasketService.GetUserBasketAsync(id);

            return Ok(Result);
        }


        /// <summary>
        /// Update user basket 
        /// </summary>
        ///<remarks>
        ///Request body must be a JSON object representing the basket
        ///
        /// Example request:
        ///
        /// {
        /// 
        ///     "id": "basket123",
        ///   
        ///   "items": [
        ///   
        ///     {
        ///       "id": 1,
        ///       
        ///       "productName": "T-Shirt",
        ///       
        ///       "pictureUrl": "img.jpg",
        ///       
        ///       "price": 200,
        ///       
        ///       "quantity": 2
        ///       
        ///     }
        ///     
        ///   ],
        ///   
        ///     "deliveryMethodId": 1
        ///     
        /// }
        /// 
        /// </remarks>  





        /// <returns>Updated basket</returns>
        /// <response code="200">Basket updated successfully</response>
        /// <response code="500">Failed to update the basket</response>
        [HttpPost]
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBasket([FromBody]BasketDto basket)
        {
          var Result= await  _serviceManager.BasketService.UpdateUserBasketAsync(basket);

            return Ok(Result);
        }
        /// <summary>
        /// Delete  user basket
        /// </summary>
        /// <param name="id">the identifier of the basket</param>
       ///<response code="200">Basket is deleted successfully</response>
       ///<response code="500">failed to delete the basket</response>
        [HttpDelete]
        [ProducesResponseType(typeof(bool),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBasket(string id)
        {
          var Result= await  _serviceManager.BasketService.DeleteUserBasketAsync(id);

            return Ok(Result);
        }
        /// <summary>
        /// Empty(clean) user basket
        /// </summary>
        /// <param name="id">The unique identifier of the basket</param>
        /// <response code="200">Basket is cleared successfully</response>
        /// <response code="404">Basket is not found</response>
        /// <response code="500">failed to clear the basket</response>
        /// <returns>An empty basket</returns>
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]

        public async Task<IActionResult> ClearBasket(string id)
        {
            var Result = await _serviceManager.BasketService.ClearBasketAsync(id);
            return Ok(Result);
        }
    }
}
