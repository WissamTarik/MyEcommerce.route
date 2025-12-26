using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.Orders
{
    public class OrderRequest
    {
        public string BasketId { get; set; }
        public OrderAddressDto ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}
