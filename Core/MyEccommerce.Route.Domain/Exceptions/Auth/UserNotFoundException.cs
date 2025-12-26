using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Auth
{
    public class UserNotFoundException(string type,string credential):NotFoundException($"user with {type} {credential} is not found")
    {
    }
}
