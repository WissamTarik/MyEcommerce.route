using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEccommerce.Route.Presentation.Attributes;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager):ControllerBase
    {
        /// <summary>
        /// Authenticate user and generate JWT token
        /// </summary>
        /// <remarks>
        /// Example request body(JSON):
        /// 
        ///  {
        ///  
        ///     "email" : "user@example.com",
        /// 
       ///      "password" : "P@ssw0rd123"
        /// 
        ///   }
        ///
        /// 
        /// </remarks>
        /// 
        /// <returns>User data with JWT token</returns>
        /// <response code="200">Login successfully</response>
        /// <response code="401">Invalid email or password</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserResultDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
          var Result= await  _serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(Result);
        }
        /// <summary>
        /// Add new user for the system and generate JWT token
        /// </summary>
        /// 
        /// <remarks>
        /// Example request body(JSON):
        /// 
        /// {
        /// 
        ///      "displayName" : "Wissam Tarik",
        ///   
        ///       "username" : "wissam_dev",
        ///   
        ///       "email" : "wissam@example.com",
        ///   
        ///       "password": "P@ssw0rd123",
        ///   
        ///       "phone": "+201234567890"
        ///    
        ///  }
        ///
        /// 
        /// </remarks>
        /// <returns>New user Data with JWT token</returns>
        /// <response code="200">Registration successful</response>
        /// <response code="400">User already exist or validation error</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
          var Result= await  _serviceManager.AuthService.RegisterAsync(registerDto);
            return Ok(Result);
        }

        /// <summary>
        ///  Check if the user email exist in the system
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>True if user exist or False if user is not exist</returns>
        /// <response code="200">Checking email is completed</response>
        [HttpGet("EmailExists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]

        public async Task<IActionResult> CheckEmailExist(string email)
        {
           var Result=await  _serviceManager.AuthService.CheckEmailExistAsync(email);

            return Ok (Result);
        }
        /// <summary>
        /// Get current logged-in user data
        /// </summary>
        /// <remarks>
        ///  The user email is extracted automatically from the JWT token.
        ///  
        /// Authorization:
        /// 
        /// - Requires JWT token
        ///
        /// 
        /// </remarks>
        /// <returns>Current user data</returns>
        /// <response code="200">User data is returned successfully</response>
        /// <response code="500">Failed to return user data</response>
        /// <response code="401">Unauthorized</response>

        [HttpGet]
        
        [Authorize]
        [ProducesResponseType(typeof(UserResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var Result=await _serviceManager.AuthService.GetCurrentUserAsync(email);
            return Ok(Result);
        }

        /// <summary>
        /// Get logged-in user address
        /// </summary>
        /// <remarks>
        ///  The user email is extracted automatically from the JWT token.
        ///  Authorization:
        /// - Requires JWT token
        /// 
        /// </remarks>
        /// <returns>The current address of logged user</returns>
        /// <response code="200">Address is returned successfully</response>
        /// <response code="404">User is not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("Address")]
        [Authorize]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var Result=await _serviceManager.AuthService.GetCurrentUserAddressAsync(email);
            return Ok(Result);
        }
        /// <summary>
        /// Update logged-in user address
        /// </summary>
        /// <remarks>
        ///  The user email is extracted automatically from the JWT token.
        ///  
        /// Authorization:
        /// 
        /// - Requires JWT token
        /// 
        ///  Example request body(JSON):
        ///  
        ///  {
        /// 
        ///     "firstName" : "Wissam",
        ///   
        ///      "lastName" : "Tarik",
        ///    
        ///      "city" : "Cairo",
        ///    
        ///      "street" : "Nasr City - Abbas El Akkad",
        ///    
        ///      "country" : "Egypt"
        ///    
        ///   }
        ///
        /// </remarks>
        /// 
        /// <returns>The updated user address</returns>
        /// <response code="200">Address is updated successfully</response>
        /// <response code="404">User is not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut("Address")]
        [Authorize]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCurrentUserAddress(AddressDto newAddress)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var Result=await _serviceManager.AuthService.UpdateCurrentUserAddressAsync(newAddress,email);
            return Ok(Result);
        }
        /// <summary>
        /// Send reset password link to email
        /// </summary>
        /// <remarks>
        /// Example of request body(JSON):
        /// 
        /// {
        /// 
        ///   "email": "user@example.com"
        /// 
         ///  }
        ///
        /// 
        /// </remarks>
        /// 
        /// <response code="200">link is sent to email successfully</response>
        [HttpPost("forget-password")]
        [ProducesResponseType(typeof(ResetPasswordResponse),StatusCodes.Status200OK)]
      public async Task<IActionResult> ForgetPassword(ForgetPasswordDto dto)
        {
           await _serviceManager.AuthService.ForgetPasswordAsync(dto.Email);
            return Ok(new ResetPasswordResponse
            {
                Message = "If email exists, the reset token has been sent."
            }
             );


        }

        /// <summary>
        /// Reset user password by using reset token
        /// </summary>
        /// <remarks>
        /// Example of request body(JSON):
        /// 
        ///  {
        ///  
        ///     "email": "user@example.com",
        ///    
        ///     "newPassword": "NewP@ssw0rd123",
        ///     
        ///     "token": "AQAAANCMnd8BFdERjHoAwE_Cl_SomeVeryLongToken"
        ///     
        ///    }
        ///
        /// 
        /// </remarks>
        /// <returns>Password reset successfully</returns>
        /// <response code="200">Password is reset successfully</response>
        /// <response code="404">User is not found</response>
        /// <response code="400">Failed to reset password</response>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            await _serviceManager.AuthService.ResetPasswordAsync(dto);
            return Ok(new { message = "Password reset successful." });
        }
        /// <summary>
        /// Get count of users in the system
        /// </summary>
        /// <remarks>
        ///  Authorization:
        /// - Requires JWT token
        /// - Allowed roles: Admin,SuperAdmin
        /// </remarks>
        /// <returns>Number of users in the systems</returns>
        /// <response code="200">Count is returned successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden(user is not an admin or superAdmin)</response>
        [HttpGet("users-count")]
        [RoleAuthorization("Admin","SuperAdmin")]
        [ProducesResponseType(typeof(UserStateDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> UsersCount()
        {
           var result= await _serviceManager.AuthService.GetUsersCountAsync();
            return Ok(result);
        }

    }
}
