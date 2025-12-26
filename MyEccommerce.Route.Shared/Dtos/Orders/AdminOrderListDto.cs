using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.Orders
{
    public class AdminOrderListDto
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public decimal   TotalPrice { get; set; }
        public string Status { get; set; }
        public string?  PaymentIntentId { get; set; }
        public DateTimeOffset   OrderDate { get; set; }
        public string DeliveryMethod { get; set; }
    }
}
