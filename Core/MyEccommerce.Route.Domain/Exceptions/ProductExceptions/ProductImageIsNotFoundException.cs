using MyEccommerce.Route.Domain.Exceptions.GlobalExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Exceptions.ProductExceptions
{
    public class ProductImageIsNotFoundException():NotFoundException("Image is not found")
    {
    }
}
