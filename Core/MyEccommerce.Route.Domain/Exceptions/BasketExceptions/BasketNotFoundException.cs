using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.BasketExceptions
{
    public class BasketNotFoundException(string id):NotFoundException($"Basket with id {id} is not found")
    {
    }
}
