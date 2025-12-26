using MyEcommerce.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Entities.Notifications
{
    public class Notification:BaseEntity<int>
    {
        public string UserEmail { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime  CreatedAt { get; set; }= DateTime.Now;
    }
}
