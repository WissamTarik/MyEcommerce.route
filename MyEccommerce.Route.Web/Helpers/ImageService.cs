using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Domain.Exceptions.ProductExceptions;

namespace MyEccommerce.Route.Web.Helpers
{
    public class ImageService(IWebHostEnvironment _webHostEnvironment) : IImageService
    {
        public void DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) throw new ProductImageIsNotFoundException();
            var filePath=Path.Combine(_webHostEnvironment.WebRootPath,imagePath.TrimStart('/'));
            if (File.Exists(filePath)) 
                File.Delete(filePath);
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            var fileName = $"{Guid.NewGuid()}{image.FileName}";

            var filePath=Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            await image.CopyToAsync(stream);
            return $"images/products/{fileName}";
        }
    }
}
