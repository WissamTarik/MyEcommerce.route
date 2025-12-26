using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.Dtos.ProductsDto
{
    public class AddProductDto
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Description is required")]
        public string Description { get; set; }
        //public string PictureUrl { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(100,100000,ErrorMessage ="Price is invalid")]
        public decimal Price { get; set; }

        public int BrandId { get; set; }
      
        public int TypeId { get; set; }

        public IFormFile Image { get; set; }
    }
}
