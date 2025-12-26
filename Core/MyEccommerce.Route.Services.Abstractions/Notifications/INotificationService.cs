using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Notifications
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userEmail, string title, string message);
    }
}
