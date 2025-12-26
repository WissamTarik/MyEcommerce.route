using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEccommerce.Route.Presentation.Attributes;
using MyEccommerce.Route.Services.Abstractions;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    ///<summary>
    ///Manage products
    ///</summary>
    public class ProductsController:ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ProductsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        /// <summary>
        /// Get all products
        /// </summary>
        /// 
        /// <remarks>
        /// Filtering:
        /// - BrandId
        /// - typeId
        /// - maxPrice/minPrice :price range
        /// 
        /// Pagination:
        /// - pageIndex (by default is 1)
        /// - pageSize(by default is 5)
        /// 
        /// Sorting:
        ///  - priceasc
        ///  - pricedesc
        ///  - name(default)
        ///  
        ///  Caching:
        ///  - Response is cached for 50 seconds
        /// </remarks>
        /// 
        /// <param name="parameters">Query parameters used for filtering and pagination</param>
        /// <response code="200">Products returned successfully</response>
        /// 
        /// <response code="500">Invalid query parameters</response>
        [HttpGet]
        [Cache(50)]
        [ProducesResponseType(typeof(PaginationResponse<ProductResponse>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
         var Result=   await _serviceManager.ProductService.GetAllProductsAsync(parameters);

            return Ok(Result);
        }
        /// <summary>
        /// Get product by id
        /// </summary>
        /// <remarks>
        /// -Retrieve a single product using its unique identifier
        /// 
        /// Errors:
        ///  -404:Product Not found
        ///  -500:Invalid Id
        /// </remarks>
        /// <param name="id">The unique Identifier for product </param>
        /// <response code="200">Product returned successfully</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Invalid identifier</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductById(int ? id)
        {
         var Result=   await _serviceManager.ProductService.GetProductByIdAsync(id);

            return Ok(Result);
        }
        /// <summary>
        /// Get all product brands
        /// </summary>
        ///  <remarks>This endpoint returns a collection of product brands
        ///  Use this method to obtain the full list of product brands supported by the
        /// system.</remarks>
        ///<response code="200">Brands returned successfully</response>
        [HttpGet("brands")]
        [ProducesResponseType(typeof(IEnumerable<ProductBrandAndTypeResponse>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBrands()
        {
         var Result=   await _serviceManager.ProductService.GetAllBrandsAsync();

            return Ok(Result);
        }

        /// <summary>
        /// Retrieves all available product types 
        /// </summary>
        /// <remarks>This endpoint returns a collection of product types.
        /// Use this method to obtain the full list of product types supported by the
        /// system.</remarks>
        ///<response code="200">product types returned successfully</response>

        [HttpGet("types")]
        [ProducesResponseType(typeof(IEnumerable<ProductBrandAndTypeResponse>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTypes()
        {
         var Result=   await _serviceManager.ProductService.GetAllTypesAsync();

            return Ok(Result);
        }

        /// <summary>
        /// Add a new product to the system
        /// </summary>
        /// <remarks>
        ///  Authorization:
        /// -Requires JWT token
        /// -Allowed roles: Admin,SuperAdmin
        ///  -The product information including name, description, price, brand, type, and image must be provided.
        /// 
        /// Example (multipart/form-data)
        ///
        /// name: Nike Air Max  
        /// description: High quality running shoes  
        /// price: 2500  
        /// brandId: 1  
        /// typeId: 2  
        /// image: nike-air-max.jpg
        ///</remarks>
        /// <param name="addProductDto">The product data to add,send as multipart/form-data, sent in the request body as JSON.</param>
        /// <response code="200">Product is added successfully</response>
        /// <response code="401">Unauthorized JWT token is invalid or missing</response>
        /// <response code="403">Forbidden user doesn't have </response>
        /// <response code="404">Product brand or type is not found</response>
        [HttpPost]
        [Authorize(Roles ="Admin,SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductResponse),StatusCodes.Status200OK)]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDto addProductDto) { 
        
         var Result=await _serviceManager.ProductService.AddProductAsync(addProductDto);
            return Ok(Result);
        }
        ///<summary>Update product data by id</summary>
        ///<param name="id">The unique identifier for product</param>
        /// <param name="updateProductDto">The product data to update,send as multipart/form-data, sent in the request body as JSON.</param>
        /// <remarks>
        /// Authorization:
        /// -Requires JWT token
        /// -Allowed roles: Admin,SuperAdmin
        ///  -The product information including name, description, price, brand, type,image is optional
        ///
        ///  **Only send fields you want to update**
        ///
        /// name: "Nike Air Max 2024"
        /// price: 2800
        /// image: nike-air-max-new.jpg
        ///
        /// **Notes:**
        /// - All fields are optional
        /// - Image replacement is optional
        /// - Authorization header is required
        /// 
        /// </remarks>
        /// <response code="200">Product is updated successfully</response>
        /// <response code="401">Unauthorized JWT token is invalid or missing</response>
        /// <response code="403">Forbidden user doesn't have </response>
        /// <response code="404">Product brand or type is not found</response>
        [HttpPut("{id}")]
        [Authorize(Roles ="Admin,SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct(int id,[FromForm] UpdateProductDto updateProductDto) { 
        
         var Result=await _serviceManager.ProductService.UpdateProductAsync(id,updateProductDto);
            return Ok(Result);
        }
        /// <summary>
        /// Delete product by Id
        /// </summary>
        /// <remarks>
        /// This endpoint deletes an existing product from the system
        /// 
        /// Authorization:
        /// -Requires JWT token
        /// -Allowed roles: Admin,SuperAdmin
        /// </remarks>
        /// <param name="id">The unique identifier of the product</param>
        /// <response code="200">true</response>
        /// <response code="401">Unauthorized JWT token is invalid or missing</response>
        /// <response code="403">Forbidden user doesn't have 
        /// the required role(Admin-SuperAdmin)</response>
        ///
        /// <response code="404">Product not found</response>
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        public async Task<IActionResult> DeleteProduct(int id)
        {
         var Result=  await  _serviceManager.ProductService.DeleteProductAsync(id);
            return Ok(Result);
        }
    }
}
