using Microsoft.AspNetCore.SignalR;
using MyEccommerce.Route.Web.Helpers;
using System.Security.Claims;

namespace MyEccommerce.Route.Web
{
    public class EmailUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
