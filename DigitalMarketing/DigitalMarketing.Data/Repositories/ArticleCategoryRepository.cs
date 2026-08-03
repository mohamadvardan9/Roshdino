
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalMarketing.DigitalMarketing.Data.Repositories
{
    public class ArticleCategoryRepository : IArticleCategoryRepository
    {
        private readonly MyDbContext _dbContext;
        public ArticleCategoryRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }




        public async Task<List<ArticleCategory>> GetAllAsync()
            => await _dbContext.ArticleCategories
            .OrderBy(x => x.Name)
            .ToListAsync();

        public async Task<ArticleCategory?> GetByIdAsync(int id)
            => await _dbContext.ArticleCategories
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ArticleCategory?> GetBySlugAsync(string slug)
            => await _dbContext.ArticleCategories
            .FirstOrDefaultAsync(x => x.Slug == slug);







        public async Task AddAsync(ArticleCategory category) => await _dbContext.ArticleCategories.AddAsync(category);
        public void UpdateAsync(ArticleCategory category) => _dbContext.ArticleCategories.Update(category);
        public void DeleteAsync(ArticleCategory category)
        {
            category.IsDeleted = true;
            _dbContext.ArticleCategories.Update(category);
        }







        public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
            => await _dbContext.ArticleCategories
            .AnyAsync(x => x.Slug == slug && (excludeId == null || x.Id !=  excludeId));
        public async Task<bool> HasArticlesAsync(int categoryId)
            => await _dbContext.Articles
            .AnyAsync(x => x.ArticleCategoryId == categoryId);

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        

        
    }
}
