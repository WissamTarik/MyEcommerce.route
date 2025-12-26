using AutoMapper;
using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Exceptions.ProductExceptions;
using MyEccommerce.Route.Services.Abstractions.Products;
using MyEccommerce.Route.Services.Specification;
using MyEccommerce.Route.Shared.Dtos.ProductsDto;
using MyEccommerce.Route.Shared.QueryParameters;
using MyEcommerce.Route.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Products
{
    public class ProductService(IUnitOfWok _unitOfWok,IMapper _mapper,IImageService _imageService) : IProductService
    {
        
      
        
        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameters parameters)
        {
            var spec = new ProductSpecification(parameters);
            var CountSpec = new ProductCountSpecification(parameters);
          var Result=   await _unitOfWok.GetRepository<int, Product>().GetAllAsync(spec);
          var Count=   await _unitOfWok.GetRepository<int, Product>().CountAsync(CountSpec);
          if(Result is null) throw new ProductBadRequestException(null);
           
            var Resp = _mapper.Map<IEnumerable<ProductResponse>>(Result);
            var Response = new PaginationResponse<ProductResponse>(Count,parameters.PageSize,parameters.PageIndex,Resp);
            return Response ;
        }
        public async Task<ProductResponse?> GetProductByIdAsync(int? id)
        {
            if (id is null) throw new ProductBadRequestException("Invalid id");
            var spec = new ProductSpecification(id.Value);
            var Result= await _unitOfWok.GetRepository<int, Product>().GetByIdAsync(spec);

            if (Result is null) throw new ProductNotFoundException(id);
            
            return _mapper.Map<ProductResponse>(Result);
        }
        public async Task<IEnumerable<ProductBrandAndTypeResponse>> GetAllBrandsAsync()
        {
            var Result = await _unitOfWok.GetRepository<int, ProductBrand>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrandAndTypeResponse>>(Result);

        }
        public async Task<IEnumerable<ProductBrandAndTypeResponse>> GetAllTypesAsync()
        {
            var Result = await _unitOfWok.GetRepository<int, ProductType>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductBrandAndTypeResponse>>(Result);
        }

        public async Task<ProductResponse> AddProductAsync(AddProductDto product)
        {
            await CheckProductBrandAndTypeExistAsync(product.BrandId, product.TypeId);



            var newProduct= _mapper.Map<Product>(product);
            newProduct.PictureUrl = await _imageService.UploadImageAsync(product.Image);
           await  _unitOfWok.GetRepository<int,Product>().AddAsync(newProduct);

           await _unitOfWok.SaveChangeAsync();
           return _mapper.Map<ProductResponse>(newProduct);
        }
        private async Task CheckProductBrandAndTypeExistAsync(int brandId,int typeId)
        {
            var brand = await _unitOfWok.GetRepository<int, ProductBrand>().GetByIdAsync(brandId)
                       ?? throw new ProductBrandNotFoundException(brandId);
            var type = await _unitOfWok.GetRepository<int, ProductType>().GetByIdAsync(typeId)
                         ?? throw new ProductTypeNotFoundException(typeId);

        }

        public async Task<ProductResponse> UpdateProductAsync(int id, UpdateProductDto productWithUpdate)
        {
            var productRepo = _unitOfWok.GetRepository<int, Product>();
            var oldProduct = await productRepo.GetByIdAsync(id)??throw new ProductNotFoundException(id);
            await CheckProductBrandAndTypeExistAsync(productWithUpdate.BrandId, productWithUpdate.TypeId);
               oldProduct.Name= productWithUpdate.Name?? oldProduct.Name;
               oldProduct.Description= productWithUpdate.Description?? oldProduct.Description;
               oldProduct.Price= productWithUpdate.Price?? oldProduct.Price;
               oldProduct.BrandId =  productWithUpdate.BrandId==0?oldProduct.BrandId:productWithUpdate.BrandId ;
               oldProduct.TypeId = productWithUpdate.TypeId==0?oldProduct.TypeId:productWithUpdate.TypeId ;
            
            if (productWithUpdate.Image is not null)
                oldProduct.PictureUrl = await _imageService.UploadImageAsync(productWithUpdate.Image);

             productRepo.Update(oldProduct);
            await _unitOfWok.SaveChangeAsync();
            return _mapper.Map<ProductResponse>(oldProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var ProductRepo = _unitOfWok.GetRepository<int, Product>();
            var product = await ProductRepo.GetByIdAsync(id) ?? throw new ProductNotFoundException(id);

            _imageService.DeleteImageAsync(product.PictureUrl);

            ProductRepo.Delete(product);
           await _unitOfWok.SaveChangeAsync();
            return true;
        }
    }
}
