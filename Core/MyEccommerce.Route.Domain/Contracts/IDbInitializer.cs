using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Contracts
{
    public interface IDbInitializer
    {
        Task InitializeDbAsync();
        Task InitializeIdentityDbAsync();
    }
}
