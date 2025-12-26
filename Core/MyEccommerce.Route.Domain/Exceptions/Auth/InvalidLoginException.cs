using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Auth
{
    public class InvalidLoginException():UnAuthorizedException("Invalid email or password !!")
    {
    }
}
