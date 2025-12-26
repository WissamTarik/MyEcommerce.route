using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEccommerce.Route.Presentation.Attributes;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Shared.Dtos.Orders;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IServiceManager _serviceManager):ControllerBase
    {
        /// <summary>
        /// Create new order for authenticated user
        /// </summary>
        /// <param name="orderRequest"></param>
        ///<remarks>
        /// Authorization:
        /// - Requires JWT token
        /// 
        /// The user email is extracted automatically from the JWT token.
        /// 
        ///  - The order information includes must be provided:
        ///   - basketId
        ///   - shippingAddress 
        ///   - deliveryMethodId 
        ///   
        /// The shippingAddress includes:
        ///  - FirstName
        ///  - LastName
        ///  - Street
        ///  - City
        ///  - Address
        ///  
        /// Example request body:
        /// 
        /// 
        /// {
        /// 
        ///     "basketId": "basket123",
        ///   
        ///      "deliveryMethodId": 1,
        ///   
        ///      "shippingAddress":
        ///       {
        ///   
        ///      "firstName": "Ahmed",
        ///     
        ///      "lastName": "Ali",
        ///     
        ///      "street": "Tahrir Street",
        ///     
        ///      "city": "Cairo",
        ///     
        ///      "country": "Egypt"
        ///     
        ///   }
        ///   
        /// }
        /// </remarks>
        /// <returns>the created order</returns>
        /// <response code="200">Order is created successfully</response>
        /// <response code="404">basket or delivery method is not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(OrderResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateOrder(OrderRequest orderRequest)
        {
            var email=User.FindFirst(ClaimTypes.Email).Value;

            var Result=await _serviceManager.OrderService.CreateOrderAsync(orderRequest, email);
            return Ok(Result);
        }
        /// <summary>
        /// Get order of authenticated user
        /// </summary>
        /// <remarks>
        /// 
        /// Authorization:
        /// - Requires JWT token
        /// 
        /// The user email is extracted automatically from the JWT token.
        /// </remarks>
        /// <param name="id">The unique identifier of the order</param>
        /// <returns>The order with specified id</returns>
        ///<response code="200">Order with specified id returned successfully </response>
        ///<response code="404">Order or user is not found </response>
        ///<response code="401">Unauthorized </response>
        [HttpGet("{id}")]
       
        [Authorize]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOrderByIdForUser(Guid id)
        {
            var email=User.FindFirst(ClaimTypes.Email).Value;

            var Result=await _serviceManager.OrderService.GetOrderByIdForUserAsync(id,email);
            return Ok(Result);
        }
        /// <summary>
        /// Get all order of authenticated user
        /// </summary>
        /// <remarks>
        /// 
        /// Authorization:
        /// - Requires JWT token
        /// 
        ///The user email is extracted automatically from the JWT token.
        /// </remarks>
  
        /// <returns>all orders of user</returns>
        ///<response code="200">All orders of specific user   returned successfully </response>
        ///<response code="404">User is not found </response>
        ///<response code="401">Unauthorized </response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetOrdersByForUser()
        {
            var email=User.FindFirst(ClaimTypes.Email).Value;

            var Result = await _serviceManager.OrderService.GetOrdersForUserAsync(email);
             return Ok(Result);

        }
        /// <summary>
        /// Get all delivery methods available
        /// </summary>
    
        /// <returns>All available delivery methods</returns>
        ///<response code="200">All delivery methods  returned successfully </response>
        ///<response code="500">Bad request </response>
        [HttpGet("DeliveryMethods")]
        [ProducesResponseType(typeof(DeliveryMethodResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {

            var Result = await _serviceManager.OrderService.GetDeliveryMethods();
             return Ok(Result);

        }
        /// <summary>
        /// Get admin orders status
        /// </summary>
        /// <remarks>
        /// 
        /// 
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        /// 
        /// Returned statistics include:
        /// - Total number of orders
        /// - Total revenue (orders subtotal + delivery cost)
        /// - Average order value
        /// - Total pending orders
        /// - Total successful orders
        /// - Total failed orders
        /// 
        /// </remarks>
        /// <returns>Order status</returns>
        ///<response code="200">Status returned successfully</response>
        ///<response code="401">Unauthorized</response>
        /// <response code="403">Forbidden (user is not Admin or SuperAdmin)</response>


        [HttpGet("orders-adminStatus")]
        [RoleAuthorization("Admin", "SuperAdmin")]
        [ProducesResponseType(typeof(AdminOrderStatusDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AdminOrderStatus()
        {
            var result = await _serviceManager.OrderService.GetAdminOrderStatusAsync();
            return Ok(result);
        }
        /// <summary>
        /// Get all orders data
        /// </summary>
        /// <param name="parameters">Query parameters used for pagination,sorting and searching</param>
        /// <remarks>
        /// 
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        /// 
        /// Sorting:
        /// - userEmail
        /// - date
        /// - priceasc
        /// - pricedesc
        /// 
        /// Pagination
        /// - pageIndex(by default is 1)
        /// - pageSize(by default is 10)
        /// </remarks>
        /// <response code="200">Orders returned successfully</response>
        /// <response code="401">UnAuthorized</response>
        /// <response code="403">user is not an Admin or superAdmin</response>
        [HttpGet("admin")]
        [RoleAuthorization("Admin", "SuperAdmin")]
        [ProducesResponseType(typeof(PaginationResponse<AdminOrderListDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOrdersForAdmin([FromQuery]AdminOrderQueryParameters parameters)
        {
           var Result=  await _serviceManager.OrderService.GetOrdersForAdminAsync(parameters);

            return Ok(Result);
        }
        /// <summary>
        /// Get order by id for Admin or superAdmin
        /// </summary>
        /// <remarks>
        /// 
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// </remarks>
        /// 
        /// <param name="id">The unique identifier or order</param>
        /// <returns>An order with specified Id</returns>
        /// <response code="200">Order is returned successfully</response>
        /// <response code="404">Order is not found</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">Forbidden (user is not an admin or superAdmin)</response>
        [HttpGet("order-details/{id}")]
        [RoleAuthorization("Admin", "SuperAdmin")]
        [ProducesResponseType(typeof(OrderResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetOrderByIdForAdmin(Guid id)
        {
           var Result=  await _serviceManager.OrderService.GetOrderByIdAsync(id);

            return Ok(Result);
        }

    }
}
