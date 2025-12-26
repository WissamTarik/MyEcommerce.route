using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Entities.Orders
{
    //part of order
    public enum OrderStatus
    {
        Pending,
        PaymentSuccess,
        PaymentFailed
        
    }
}
