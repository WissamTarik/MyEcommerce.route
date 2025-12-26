using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Domain.Exceptions.Auth;
using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using MyEccommerce.Route.Services.Abstractions.Auth;
using MyEccommerce.Route.Services.Abstractions.Emails;
using MyEccommerce.Route.Shared.Dtos.AuthDtos;
using MyEccommerce.Route.Shared.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyEccommerce.Route.Services.Auth
{
    public class AuthService(UserManager<AppUser> _userManager,IOptions<JwtOptions> _options,IMapper _mapper,IEmailService _emailService,IConfiguration _configuration) : IAuthService
    {
        public async Task<bool> CheckEmailExistAsync(string email)
        {
           var user=await _userManager.FindByEmailAsync(email);
            if (user is null) return false;
            return true;
        }

        public async Task<AddressDto?> GetCurrentUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user is null) throw new UserNotFoundException("email",email);

            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDto?> GetCurrentUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new AuthBadRequestException();
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateToken(user)
            };
        }
        public async Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto newAddress, string email)
        {
            var user = await _userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Email.ToLower()==email.ToLower());
            if (user is null) throw new AuthBadRequestException();
            if(user.Address is null)
            {
                user.Address=_mapper.Map<Address>(newAddress);
            }
            else
            {
                user.Address.FirstName = newAddress.FirstName;
                user.Address.LastName = newAddress.LastName;
                user.Address.City = newAddress.City;
                user.Address.Street = newAddress.Street;
                user.Address.Country = newAddress.Country;
               
            }
            var Result= await   _userManager.UpdateAsync(user);
            if (!Result.Succeeded) throw new AuthBadRequestException();
            return _mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new InvalidLoginException();
           var Flag= await _userManager.CheckPasswordAsync(user, loginDto.Password);
           if(!Flag) throw new InvalidLoginException();
            return new UserResultDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateToken(user)
            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user is not null) throw new ExistUserException();
            var newUser = new AppUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                PhoneNumber = registerDto.Phone,
                UserName = registerDto.username
            };
          var Result=  await _userManager.CreateAsync(newUser, registerDto.Password);
            if (!Result.Succeeded)
            {
                throw new ValidationException(Result.Errors.Select(e => e.Description));
            }
            return new UserResultDto()
            {
                DisplayName = newUser.DisplayName,
                Email = newUser.Email,
                Token = await GenerateToken(newUser)
            };
        
        
        }

       
     

        public async Task ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) throw new UserNotFoundException("email",email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

         var encodedToken = HttpUtility.UrlEncode(token);
            var resetUrl = $"{_configuration.GetSection("Frontend")["BaseUrl"]}/reset-password?email={email}&token={encodedToken}";

            var message = $@"
        <h3>Password Reset Request</h3>
        <p>Your reset token:</p>
        <a href='{resetUrl}'>{resetUrl}</a>";
            
            await _emailService.SendEmailAsync(email, message, "Reset your password");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordData)
        {
            var user=await _userManager.FindByEmailAsync(resetPasswordData.Email);
            if (user is null) throw new UserNotFoundException("email",resetPasswordData.Email);

            var decodedToken=HttpUtility.UrlDecode(resetPasswordData.Token);
            if (string.IsNullOrEmpty(decodedToken)) throw new BadRequestException("Invalid operation");
          var result=await  _userManager.ResetPasswordAsync(user, decodedToken, resetPasswordData.NewPassword);
            if (!result.Succeeded) throw new ValidationException(result.Errors.Select(e => e.Description));


        
        }

        public async Task<UserStateDto> GetUsersCountAsync()
        {
            var count= await _userManager.Users.CountAsync();
            return new UserStateDto() { TotalUsers = count };
        }

        private async Task<string> GenerateToken(AppUser user)
        {
            var jwtOptions = _options.Value;
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
