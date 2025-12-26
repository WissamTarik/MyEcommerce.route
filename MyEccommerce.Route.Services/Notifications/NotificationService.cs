using Microsoft.AspNetCore.SignalR;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Entities.Notifications;
using MyEccommerce.Route.Persistence.Notifications.Hubs;
using MyEccommerce.Route.Services.Abstractions.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Notifications
{
    public class NotificationService(IUnitOfWok _unitOfWok,IHubContext<NotificationHub> _hubContext) : INotificationService
    {
        public async Task SendNotificationAsync(string userEmail, string title, string message)
        {
            var notification = new Notification()
            {
                UserEmail = userEmail,
                Title = title,
                Message = message
            };

          await  _unitOfWok.GetRepository<int,Notification>().AddAsync(notification);
            await _unitOfWok.SaveChangeAsync();

            //push the message(Real-time)
           await _hubContext.Clients.User(userEmail).SendAsync("ReceiveMessage", new
            {
               title,
               message,
               createdAt=notification.CreatedAt
            });
        }
    }
}
