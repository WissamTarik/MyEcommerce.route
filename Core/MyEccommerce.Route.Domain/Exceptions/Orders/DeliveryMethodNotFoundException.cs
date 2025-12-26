using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Orders
{
    public class DeliveryMethodNotFoundException(int id):NotFoundException($"Delivery method with id {id} is not found ")
    {
    }
}
