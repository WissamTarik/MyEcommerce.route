using MyEccommerce.Route.Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Auth
{
    public interface IAuthService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> CheckEmailExistAsync(string email);
        Task<UserResultDto?> GetCurrentUserAsync(string email);
        Task<AddressDto?> GetCurrentUserAddressAsync(string email);
        Task<AddressDto?> UpdateCurrentUserAddressAsync(AddressDto newAddress,string email);
        Task ForgetPasswordAsync(string email);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordData);
        Task<UserStateDto> GetUsersCountAsync();

    }
}
