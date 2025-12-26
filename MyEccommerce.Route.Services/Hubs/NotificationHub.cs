using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence.Notifications.Hubs
{
    [Authorize]
    public class NotificationHub:Hub
    {
    }
}
