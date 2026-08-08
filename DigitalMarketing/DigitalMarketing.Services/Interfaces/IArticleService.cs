using DigitalMarketing.DigitalMarketing.Services.Common;
using DigitalMarketing.DigitalMarketing.Services.DTOs.ArticleDtos;

namespace DigitalMarketing.DigitalMarketing.Services.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetAllAsync();
        Task<List<ArticleDto>> GetPublishedAsync();
        Task<ArticleDto?> GetByIdAsync(int id);
        Task<ArticleDto?> GetBySlugAsync(string slug);
        Task<List<ArticleDto>> GetByCategoryAsync(int categoryId);

        Task<ServiceResult> CreateAsync(CreateArticleDto dto);
        Task<ServiceResult> UpdateAsync(UpdateArticleDto dto, Action<string>? deleteOldCoverCallback = null);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult> TogglePublishAsync(int id);
    }
}
