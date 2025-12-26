using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Emails
{
    public interface IEmailService
    {
         Task SendEmailAsync(string toEmail,string body,string subject);
    }
}
