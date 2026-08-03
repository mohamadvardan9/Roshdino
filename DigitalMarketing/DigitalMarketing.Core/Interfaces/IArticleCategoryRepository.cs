using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IArticleCategoryRepository   
    {
        Task<List<ArticleCategory>> GetAllAsync();
        Task<ArticleCategory?> GetByIdAsync(int id);
        Task<ArticleCategory?> GetBySlugAsync(string slug);


        Task AddAsync(ArticleCategory category);
        void UpdateAsync(ArticleCategory category);
        void DeleteAsync(ArticleCategory category);


        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        Task<bool> HasArticlesAsync(int categoryId);
        Task SaveChangesAsync();
    }
}
