using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.BasketDtos
{
    public class BasketDto
    {
        /// <summary>
        /// The unique identifier of basket
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Items in basket
        /// </summary>
        public IEnumerable<BasketItemDto> Items { get; set; }
        /// <summary>
        /// Selected method id
        /// </summary>
        public int? DeliveryMethodId { get; set; }
        /// <summary>
        ///  The shipping cost associated with the order.
        /// </summary>
        public decimal? ShippingCost { get; set; }
        /// <summary>
        /// Stripe payment intentId
        /// </summary>
        public string? PaymentIntentId { get; set; }
        /// <summary>
        /// Stripe client secret used by frontend
        /// </summary>
        public string? ClientSecret { get; set; }
    }
}
