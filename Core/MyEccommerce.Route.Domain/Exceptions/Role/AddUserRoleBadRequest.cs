using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Role
{
    public class AddUserRoleBadRequest(string roleName,string userId):BadRequestException($"Failed to add {roleName} to UserId {userId}")
    {
    }
}
