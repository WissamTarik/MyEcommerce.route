using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.QueryParameters
{
    public class AdminOrderQueryParameters
    {
        /// <summary>
        /// Gets or sets the current zero-based page index for paginated results.
        /// -By default is 1
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// Gets or sets the number of items to include on each page of results.
        /// -By default is 10
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// Search for order by its userEmail(case in-sensitive)
        /// </summary>
       

        public string? Search { get; set; }
        /// <summary>
        /// Sort orders by (userEmail-priceasc-pricedesc-date(default))
        /// </summary>
        public string? Sort { get; set; }
    }
}
