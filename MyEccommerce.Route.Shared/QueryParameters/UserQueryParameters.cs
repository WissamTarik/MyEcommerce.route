using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Shared.QueryParameters
{
    public class UserQueryParameters
    {
        /// <summary>
        /// Gets or sets the current zero-based page index for paginated results.
        /// - By default is 1
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// Gets or sets the number of items to include on each page of results.
        /// - By default is 5
        /// </summary>
        public int PageSize { get; set; } = 5;
        /// <summary>
        /// Search for user by its email(case in-sensitive)
        /// </summary>
        ///
        public string? Search { get; set; }
        /// <summary>
        /// Sort users by (email-name(default))
        /// </summary>
        public string? Sort { get; set; }
    }
}
