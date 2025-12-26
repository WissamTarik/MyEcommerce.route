using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Orders
{
    public class OrderNotFoundException(Guid id):NotFoundException($"Order with id: {id} is not found")
    {
    }
}
