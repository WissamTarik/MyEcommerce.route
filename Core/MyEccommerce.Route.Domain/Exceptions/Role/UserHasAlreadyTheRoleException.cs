using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Role
{
    public class UserHasAlreadyTheRoleException(string userId,string roleName):AlreadyExistException($"User with Id:{userId} already has role { roleName}")
    {
    }
}
