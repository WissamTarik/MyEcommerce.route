using MyEccommerce.Route.Shared.Dtos.Admin;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Admin
{
    public interface IAdminService
    {
        Task<PaginationResponse<AdminUserDto>> GetAllUsersAsync(UserQueryParameters parameters);
        Task<AdminUserDto> CreateUserByAdminAsync(CreateUserDto userDto);
        Task <string> UpdateUserRoleAsync(string userId, string role);
        Task  DeleteUserAsync(string userId);
    
    }
}
