using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Shared.Dtos.BasketDtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IServiceManager _serviceManager):ControllerBase
    {
        /// <summary>
        /// Create or update payment intent for basket
        /// </summary>
        /// <remarks>
        /// 
        ///Authorization:
        /// - Requires JWT token
        ///
        /// 
        ///The returned basket includes:
        /// - Updated PaymentIntentId
        /// - ClientSecret
        /// - Shipping cost
        /// </remarks>
        /// <param name="basketId">The unique identifier of the basket</param>
        /// <returns>basket with payment intent</returns>
        /// <response code="200">basket returned with payment intent successfully</response>
        /// <response code="404">basket or product is not found</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpPost("{basketId}")]
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            var Result = await _serviceManager.PaymentService.CreatePaymentIntentAsync(basketId);
            return Ok(Result);

        }
        [Route("webhook")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
             //const string endpointSecret = "whsec_4c3dfdff9e799fcdf0796d9c55e14119042d29750b75848553384a339408b952";
           
                var signatureHeader = Request.Headers["Stripe-Signature"];
            await _serviceManager.PaymentService.UpdateOrderPaymentStatusAsync(json, signatureHeader);
                //var stripeEvent = EventUtility.ParseEvent(json);

                //stripeEvent = EventUtility.ConstructEvent(json,
                //        signatureHeader, endpointSecret);

                //// If on SDK version < 46, use class Events instead of EventTypes
                //if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                //{
                //   // var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                //    //Console.WriteLine("A successful payment for {0} was made.", paymentIntent.Amount);
                //    // Then define and call a method to handle the successful payment intent.
                //    // handlePaymentIntentSucceeded(paymentIntent);
                //}
                //else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
                //{
                //   // var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                //    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                //    // handlePaymentMethodAttached(paymentMethod);
                //}
                //else
                //{
                //    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                //}
                return Ok();
            }
           
           
        

    }
}
