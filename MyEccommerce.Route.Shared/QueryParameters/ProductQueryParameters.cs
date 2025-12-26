using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.QueryParameters
{
    public class ProductQueryParameters
    {
        /// <summary>
     /// Filter by Brand Id
     /// </summary>
     /// 
        public int? BrandId { get; set; }
        /// <summary>
        /// Filter by typeId
        /// </summary>

        public int? TypeId { get; set; }
        /// <summary>
        /// Filter by minimum Price
        /// </summary>
        public decimal? MinPrice { get; set; }
        /// <summary>
        /// Filter by maximum price
        /// </summary>
        public decimal? MaxPrice { get; set; }
        /// <summary>
        /// Sort products by (priceasc-pricedesc-name(default))
        /// </summary>
        public string? Sort { get; set; }
        /// <summary>
        /// Search for product by its name(case in-sensitive)
        /// </summary>
        ///
   

        public string? Search { get; set; }
        /// <summary>
        /// Gets or sets the number of items to include on each page of results.
        /// -By default is 5
        /// </summary>
        /// 


        public int PageSize { get; set; } = 5;
        /// <summary>
        /// Gets or sets the current zero-based page index for paginated results.
        /// -By default is 1
        /// </summary>
        
        public int  PageIndex { get; set; } = 1;
    }
}
