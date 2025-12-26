using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.Auth
{
    public class ValidationException(IEnumerable<string> errors):Exception("Validation Error")
    {
        public IEnumerable<string> Errors { get; } = errors;
    }
}
