using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleCategoryDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Interfaces
{
    public interface IArticleCategoryService
    {
        Task<List<ArticleCategoryDto>> GetAllAsync();
        Task<ArticleCategoryDto?> GetByIdAsync(int id);
        Task<ArticleCategoryDto?> GetBySlugAsync(string slug);

        Task<ServiceResult> CreateAsync(CreateArticleCategoryDto dto);
        Task<ServiceResult> UpdateAsync(UpdateArticleCategoryDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
