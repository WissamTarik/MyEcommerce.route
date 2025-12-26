using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.RolesDtos
{
    public class AddOrRemoveRoleToUserRequest
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
