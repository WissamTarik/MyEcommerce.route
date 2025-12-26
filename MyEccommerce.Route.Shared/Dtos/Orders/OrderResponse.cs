using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        IEnumerable<OrderItemDto> Items { get; set; }

        public string DeliveryMethod { get; set; }//DeliveryMethod name
        public string OrderStatus { get; set; }//DeliveryMethod name
        public OrderAddressDto OrderAddress { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}
