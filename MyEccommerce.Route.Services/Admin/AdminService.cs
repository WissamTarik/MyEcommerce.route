using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Domain.Entities.Orders;
using MyEccommerce.Route.Domain.Exceptions.Auth;
using MyEccommerce.Route.Domain.Exceptions.Role;
using MyEccommerce.Route.Services.Abstractions.Admin;
using MyEccommerce.Route.Services.Specification;
using MyEccommerce.Route.Shared.Dtos.Admin;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Admin
{
    public class AdminService(UserManager<AppUser> _userManager,RoleManager<IdentityRole> _roleManager) : IAdminService
    {
      

        public async Task<PaginationResponse<AdminUserDto>> GetAllUsersAsync(UserQueryParameters parameters)
        {
            var query =  _userManager.Users.AsQueryable();
            var result = await new AdminUserSpecification(parameters).AllApplies(query).ToListAsync();
            
            var count=await query.CountAsync();
            var adminUsers = new List<AdminUserDto>();
            foreach (var u in result)
            {
                var roles=await _userManager.GetRolesAsync(u);

                adminUsers.Add(new AdminUserDto()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Roles = roles.ToList()
                });
            }
            return new PaginationResponse<AdminUserDto>(count, parameters.PageSize, parameters.PageIndex, adminUsers);
        }

        public async Task<AdminUserDto> CreateUserByAdminAsync(CreateUserDto userDto)
        {
            var user= await _userManager.FindByEmailAsync(userDto.Email);
            if (user is not null) throw new ExistUserException();

            var newUser = new AppUser()
            {
                DisplayName=userDto.DisplayName,
                Email = userDto.Email,
                UserName=userDto.Username
            };

           var result=await   _userManager.CreateAsync(newUser, userDto.Password) ;
            if (!result.Succeeded) throw new ValidationException(result.Errors.Select(e => e.Description));
            if(!await _roleManager.RoleExistsAsync(userDto.Role))
            {
                throw new RoleNotFoundException(userDto.Role);
            }
           await  _userManager.AddToRoleAsync(newUser, userDto.Role);
            var roles = await _userManager.GetRolesAsync(newUser);
            return new AdminUserDto()
            {
                Email = newUser.Email,
                Id = newUser.Id,
                Roles =roles.ToList()
            };
        }

        public async Task<string> UpdateUserRoleAsync(string userId, string role)
        {
          var user=await  _userManager.FindByIdAsync(userId)??throw new UserNotFoundException("Id",userId);

            var currentRoles=await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if(!await _roleManager.RoleExistsAsync(role))
            {
                throw new RoleNotFoundException(role);
            }
          var Result=   await _userManager.AddToRoleAsync(user,role);
            if (!Result.Succeeded) throw new ValidationException(Result.Errors.Select(e=>e.Description));

            return "Updated!!";
           
        }

        public async Task DeleteUserAsync(string userId)
        {
           var user=await _userManager.FindByIdAsync(userId)?? throw new UserNotFoundException("Id",userId);

          var result=await  _userManager.DeleteAsync(user);
            if (!result.Succeeded) throw new ValidationException(result.Errors.Select(e => e.Description));
        }
    }
}

