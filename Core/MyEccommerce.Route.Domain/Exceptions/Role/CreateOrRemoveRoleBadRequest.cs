using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Role
{
    public class CreateOrRemoveRoleBadRequest():BadRequestException("Failed to create or remove role")
    {
    }
}
