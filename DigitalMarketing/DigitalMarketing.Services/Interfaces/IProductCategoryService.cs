using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ProductCategoryDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategoryDto>> GetAllAsync();
        Task<ProductCategoryDto?> GetByIdAsync(int id);
        Task<ProductCategoryDto?> GetBySlugAsync(string slug);


        Task<ServiceResult> CreateAsync(CreateProductCategoryDto dto);
        Task<ServiceResult> UpdateAsync(UpdateProductCategoryDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
