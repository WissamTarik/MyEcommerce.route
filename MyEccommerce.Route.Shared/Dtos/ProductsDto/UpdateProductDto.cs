using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.ProductsDto
{
    public class UpdateProductDto
    {
     
        public string? Name { get; set; }
        public string? Description { get; set; }
        //public string PictureUrl { get; set; }
      
        public decimal? Price { get; set; }

        public int BrandId { get; set; }

        public int TypeId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
