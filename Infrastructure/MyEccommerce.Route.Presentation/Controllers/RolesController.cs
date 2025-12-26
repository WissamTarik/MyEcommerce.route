using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEccommerce.Route.Presentation.Attributes;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Shared.Dtos.RolesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController(IServiceManager _serviceManager):ControllerBase
    {
        /// <summary>
        /// Add new role to the system
        /// </summary>
        /// <remarks>
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        ///  Example request body:
        /// 
        ///    {
        ///  
        ///     "roleName": "Admin"
        ///   
        ///     }
        /// </remarks>
        /// <returns>true the role id added successfully</returns>
        /// <response code="200">Role is added successfully</response>
        /// <response code="500">Failed to create role</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(User is not an admin or superAdmin)</response>
        [HttpPost]
        [RoleAuthorization("Admin","SuperAdmin")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddNewRole(CreateRoleRequest role)
        {
            var Result= await _serviceManager.RoleService.AddRoleAsync(role);

            return Ok(Result);
        
        }
        /// <summary>
        /// Assign a role to the user
        /// </summary>
        /// <remarks>
        ///  Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        /// Example request body:
        /// 
        ///   {
        ///  
        ///     "roleName": "Admin",
        ///      "userId":"a123"
        ///   
        ///   }
        /// </remarks>
        /// <returns>Role is added to user successfully</returns>
        /// <response code="200">Role is added successfully</response>
        /// <response code="404">User or Role  not found</response>
        /// <response code="409">User already has the role</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(user is not an admin or superAdmin)</response>
        [HttpPost("assign")]
        [RoleAuthorization("Admin", "SuperAdmin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> AddRoleToUser(AddOrRemoveRoleToUserRequest addRoleRequest)
        {
            var Result= await _serviceManager.RoleService.AssignRoleToUserAsync(addRoleRequest);

            return Ok(Result);
        
        }
        /// <summary>
        /// Remove role from a user
        /// </summary>
        /// <remarks>
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        /// Example request body:
        /// 
        ///   {
        ///  
        ///     "roleName": "Admin",
        ///      "userId":"123@a"
        ///   
        ///   }
        /// </remarks>
        /// <returns>Role is removed from the user successfully!!</returns>
        /// <response code="200">Role is removed from user successfully</response>
        /// <response code="404">Role or User is not found</response>
        /// <response code="500">Failed to remove role from user</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(user is not an admin or superAdmin)</response>
        [HttpPost("remove")]
        [RoleAuthorization("Admin", "SuperAdmin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveRoleFromUser(AddOrRemoveRoleToUserRequest deleteRoleRequest)
        {
            var Result= await _serviceManager.RoleService.RemoveRoleFromUserAsync(deleteRoleRequest);

            return Ok(Result);
        
        }

        /// <summary>
        /// Get all users with their roles
        /// </summary>
        /// <remarks>
        /// Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// </remarks>
        /// <returns>All users with roles</returns>
        /// <response code="200">All users with their roles is returned successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden (user is not an admin or superAdmin)</response>
        [HttpGet("usersRoles")]
        [RoleAuthorization("Admin", "SuperAdmin")]
        [ProducesResponseType(typeof(List<UserWithRolesDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> GetUsersWithRoles()
        {
            var Result= await _serviceManager.RoleService.GetAllUsersWithRolesAsync();

            return Ok(Result);
        
        }
        /// <summary>
        /// Get all roles assigned to specific user 
        /// </summary>
        /// <remarks>
        ///Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// 
        ///Example request body:
        /// 
        ///   {
        ///  
        ///     "email": "Admin@hotmail.com"
        ///   
        ///   }
        /// </remarks>
        /// <returns>Roles assigned to user</returns>
        /// <response code="200">Roles is returned successfully</response>
        /// <response code="404">User is not found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden (user is not an admin or superAdmin)</response>
        [HttpPost("userRoles")]
        [RoleAuthorization("Admin", "SuperAdmin")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserWithRoles(GetUserRolesRequest userRolesRequest)
        {
            var Result= await _serviceManager.RoleService.GetUserRolesAsync(userRolesRequest.Email);

            return Ok(Result);
        
        }
    }
}
