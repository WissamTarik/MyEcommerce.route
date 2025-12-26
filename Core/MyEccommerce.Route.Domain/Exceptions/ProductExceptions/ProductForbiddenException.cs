using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.ProductExceptions
{
    public class ProductForbiddenException():ForbiddenException("You don't have the permission to add,update or delete product")
    {
    }
}
