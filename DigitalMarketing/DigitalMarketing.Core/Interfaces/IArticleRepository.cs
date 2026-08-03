using DigitalMarketing.DigitalMarketing.Core.Entities;

namespace DigitalMarketing.DigitalMarketing.Core.Interfaces
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetAllAsync();
        Task<List<Article>> GetPublishedAsync();
        Task<Article?> GetByIdAsync(int id);
        Task<Article?> GetBySlugAsync(string slug);
        Task<List<Article>> GetByCategoryAsync(int categoryId);

        Task AddAsync(Article article);
        void Update(Article article);
        void Delete(Article article);

        Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
        Task SaveChangesAsync();
    }
}
