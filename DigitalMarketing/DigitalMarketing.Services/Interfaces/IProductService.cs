using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<List<ProductDto>> GetPublishedAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto?> GetBySlugAsync(string slug);
        Task<List<ProductDto>> GetByCategoryAsync(int categoryId);




        Task<ServiceResult> CreateAsync(CreateProductDto dto);
        Task<ServiceResult> UpdateAsync(UpdateProductDto dto);
        Task<ServiceResult> DeleteAsync(int id);





        Task<ServiceResult> RemoveImageAsync(int imageId, int productId, Action<string> deleteFileCallback);
        Task<ServiceResult> SetMainImageAsync(int productId, int imageId);
        Task<ServiceResult> TogglePublishAsync(int id);



    }
}
