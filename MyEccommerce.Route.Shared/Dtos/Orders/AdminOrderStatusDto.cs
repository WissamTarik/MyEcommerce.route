using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.Orders
{
    public class AdminOrderStatusDto
    {
        public int TotalOrders { get; set; }
        public int TotalPendingOrders { get; set; }
        public int TotalSuccessOrders { get; set; }
        public int TotalFailedOrders { get; set; }
        public decimal OrdersAverage { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
