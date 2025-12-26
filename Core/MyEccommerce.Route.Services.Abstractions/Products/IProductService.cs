using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Abstractions.Products
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters parameters);
        Task<ProductResponse?> GetProductByIdAsync(int ? id);
        Task<IEnumerable<ProductBrandAndTypeResponse>> GetAllBrandsAsync();
        Task<IEnumerable<ProductBrandAndTypeResponse>> GetAllTypesAsync();
        Task<ProductResponse> AddProductAsync(AddProductDto product);
        Task<ProductResponse> UpdateProductAsync(int id,UpdateProductDto productWithUpdate);
        Task<bool> DeleteProductAsync(int id);
    }
}
