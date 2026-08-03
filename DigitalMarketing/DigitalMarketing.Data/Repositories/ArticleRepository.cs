using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalMarketing.DigitalMarketing.Data.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MyDbContext _dbContext;
        public ArticleRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }






        public async Task<List<Article>> GetAllAsync()
        {
            return await _dbContext.Articles
                .Include(x => x.ArticleCategory)
                .OrderByDescending(x => x.PublishedAt)
                .ToListAsync();
        }

        public async Task<List<Article>> GetPublishedAsync()
        {
            return await _dbContext.Articles
                .Where(x => x.IsPublished)
                .Include(x => x.ArticleCategory)
                .OrderByDescending(x => x.PublishedAt)
                .ToListAsync();
        }


        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _dbContext.Articles
                .Include(x => x.ArticleCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<Article?> GetBySlugAsync(string slug)
        {
            return await _dbContext.Articles
                .Include(x => x.ArticleCategory)
                .FirstOrDefaultAsync(x => x.Slug == slug && x.IsPublished);
        }


        public async Task<List<Article>> GetByCategoryAsync(int categoryId)
        {
            return await _dbContext.Articles
                .Where(x => x.ArticleCategoryId == categoryId && x.IsPublished)
                .OrderByDescending(x => x.PublishedAt)
                .ToListAsync();
        }






        public async Task AddAsync(Article article) => await _dbContext.Articles.AddAsync(article);
        public void Update(Article article) => _dbContext.Articles.Update(article);
        public void Delete(Article article)
        {
            article.IsDeleted = true;
            _dbContext.Articles.Update(article);
        }






        public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
            => await _dbContext.Articles.AnyAsync(x => x.Slug == slug && (excludeId == null || x.Id != excludeId));
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    }
}
