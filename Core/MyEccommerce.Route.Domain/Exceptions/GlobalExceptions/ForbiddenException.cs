using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.GlobalExceptions
{
    public class ForbiddenException(string message):Exception(message)
    {
    }
}
