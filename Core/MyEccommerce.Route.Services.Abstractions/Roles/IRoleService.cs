using MyEccommerce.Route.Shared.Dtos.RolesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Roles
{
    public interface IRoleService
    {
        Task<bool>AddRoleAsync(CreateRoleRequest createRole);
        Task<string> AssignRoleToUserAsync(AddOrRemoveRoleToUserRequest addRole);
        Task<string> RemoveRoleFromUserAsync(AddOrRemoveRoleToUserRequest removeRoleFromUserRequest);
        Task<List<UserWithRolesDto>> GetAllUsersWithRolesAsync();
        Task<List<string>> GetUserRolesAsync(string userId);
    }
}
