using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEccommerce.Route.Presentation.Attributes;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Shared.Dtos.Admin;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IServiceManager _serviceManager):ControllerBase
    {
        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <remarks>
        ///  Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        /// Pagination:
        /// - pageIndex(by default is 1)
        /// - pageSize(by default is 5)
        /// 
        /// Sorting:
        ///  - emailasc
        ///  - emaildesc
        ///  - name(default)
        ///  
        /// Search:
        ///  - Search by email
        /// </remarks>
        /// <param name="parameters">Query parameters used for pagination ,sorting and searching</param>
        /// <returns>All user in the system</returns>
        /// <response code="200">Users returned successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(user is not an admin or superAdmin)</response>
        [HttpGet]
        [RoleAuthorization("Admin","SuperAdmin")]
        [ProducesResponseType(typeof(PaginationResponse<AdminUserDto>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsers([FromQuery]UserQueryParameters parameters)
         {
         var Result=  await  _serviceManager.AdminService.GetAllUsersAsync(parameters);
            return Ok(Result);
        }
        /// <summary>
        /// Create user by admin or superAdmin
        /// </summary>
        /// <param name="userDto">User data required to create new user</param>
        /// <remarks>
        ///  Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        /// NOTES:
        /// - Email must be unique
        /// - Password must meet Identity password requirements
        /// - Role must already exist in the system
        /// - Default role is "User" if not provided
        /// 
        /// Example request body:
        /// 
        ///   {
        /// 
        ///      "displayName": "Ahmed Ali",
        ///   
        ///      "email": "ahmed@example.com",
        ///   
        ///      "username": "ahmed_ali",
        ///   
        ///      "password": "Pa$$w0rd123",
        ///   
        ///      "role": "User"
        ///   
        ///   }
        /// </remarks>
        /// <returns>The user created </returns>
        /// <response code="200">User is created successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(user is not an admin or superAdmin)</response>
        /// <response code="500">Failed to create the user(role is not exist,user is already exist)</response>
        /// <response code="400">Failed to create the user</response>
        [HttpPost]
        [RoleAuthorization("Admin","SuperAdmin")]
        [ProducesResponseType(typeof(AdminUserDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
         {
         var Result=  await  _serviceManager.AdminService.CreateUserByAdminAsync(userDto);
            return Ok(Result);
        }
        /// <summary>
        /// Update user role 
        /// </summary>
        /// <remarks>
        /// It delete all user roles and substitute them by given role
        /// 
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        /// NOTE:
        /// - Role must be already exist
        /// 
        ///  Example request body:
        /// 
        ///   {
        ///  
        ///     "roleName": "Admin"
        ///   
        ///   }
        /// </remarks>
        /// <param name="id">The unique identifier for the user</param>
        /// 
        /// <returns>Updated !! </returns>
        /// <response code="200">Role updated successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(user is not an admin or superAdmin)</response>
        /// <response code="404">Role or user is not found</response>
        [HttpPut("role/{id}")]
        [RoleAuthorization("Admin","SuperAdmin")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateRole(string id,[FromBody] UpdateUserRoleByAdminDto updateRole)
         {
         var Result=  await  _serviceManager.AdminService.UpdateUserRoleAsync(id,  updateRole.RoleName);
            return Ok(Result);
        }
        /// <summary>
        /// Delete user from the system by Admin or superAdmin
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <remarks>
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// </remarks>
        /// <response code="200">User is deleted successfully</response>
        /// <response code="404">User is not found</response>
        /// <response code="400">Failed to delete the user</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(user is not an admin or superAdmin)</response>
        [HttpDelete("{id}")]
        [RoleAuthorization("Admin","SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUser(string id)
         {
           await  _serviceManager.AdminService.DeleteUserAsync(id);
            return Ok();
        }
    }
}
