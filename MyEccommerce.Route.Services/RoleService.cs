using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Domain.Exceptions.Auth;
using MyEccommerce.Route.Domain.Exceptions.Role;
using MyEccommerce.Route.Services.Abstractions.Roles;
using MyEccommerce.Route.Shared.Dtos.RolesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services
{
    public class RoleService(UserManager<AppUser> _userManager,RoleManager<IdentityRole> _roleManager) : IRoleService
    {
        public async Task<bool> AddRoleAsync(CreateRoleRequest createRole)
        {
            if (await _roleManager.RoleExistsAsync(createRole.RoleName)) 
                 throw new RoleAlreadyExistException(createRole.RoleName);

          var result=await   _roleManager.CreateAsync(new IdentityRole() { Name = createRole.RoleName });
            if (!result.Succeeded) throw new CreateOrRemoveRoleBadRequest();
            return result.Succeeded;
        }

        public async Task<string> AssignRoleToUserAsync(AddOrRemoveRoleToUserRequest addRole)
        {
            var user= await _userManager.FindByIdAsync(addRole.UserId)??throw new UserNotFoundException("id",addRole.UserId);
            var role=await _roleManager.FindByNameAsync(addRole.RoleName)?? throw new RoleNotFoundException(addRole.RoleName);
            if (await _userManager.IsInRoleAsync(user, addRole.RoleName)) throw new UserHasAlreadyTheRoleException(addRole.UserId, addRole.RoleName);
           var Result=await  _userManager.AddToRoleAsync(user, addRole.RoleName);
            if (!Result.Succeeded) throw new AddUserRoleBadRequest(addRole.RoleName, addRole.UserId);
            return "Role is added successfully";
        }


        public async Task<string> RemoveRoleFromUserAsync(AddOrRemoveRoleToUserRequest removeRoleFromUserRequest)
        {
            var user = await _userManager.FindByIdAsync(removeRoleFromUserRequest.UserId) ?? throw new UserNotFoundException("id", removeRoleFromUserRequest.UserId);
            var role = await _roleManager.FindByNameAsync(removeRoleFromUserRequest.RoleName) ?? throw new RoleNotFoundException(removeRoleFromUserRequest.RoleName);
            var Result=await _userManager.RemoveFromRoleAsync(user, removeRoleFromUserRequest.RoleName);
            if (!Result.Succeeded) throw new CreateOrRemoveRoleBadRequest();
            return "Role is removed from the user successfully!!";
        }


        public async Task<List<UserWithRolesDto>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRole = new List<UserWithRolesDto>();
            foreach (var user in users)
            {
                var roles =await _userManager.GetRolesAsync(user);
                var userRoles = new UserWithRolesDto()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList()
                };
                usersWithRole.Add(userRoles);
            }
            return usersWithRole;
        }
    
    public async Task<List<string>>GetUserRolesAsync(string userEmail)
        {
            var user =await _userManager.FindByEmailAsync(userEmail)?? throw new UserNotFoundException("email", userEmail);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    
    }
}
