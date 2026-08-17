using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalMarketing.DigitalMarketing.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly MyDbContext _dbContext;
        public ProductCategoryRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }







        public async Task<List<ProductCategory>> GetAllAsync()
            => await _dbContext.ProductCategories
                .OrderBy(x => x.Name)
                .ToListAsync();

        public async Task<ProductCategory?> GetByIdAsync(int id)
            => await _dbContext.ProductCategories
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ProductCategory?> GetBySlugAsync(string slug)
            => await _dbContext.ProductCategories
            .FirstOrDefaultAsync(x => x.Slug == slug);







        public async Task AddAsync(ProductCategory category) => await _dbContext.AddAsync(category);
        public void Update(ProductCategory category) => _dbContext.Update(category);
        public void Delete(ProductCategory category)
        {
            category.IsDeleted = true;
            _dbContext.Update(category);
        }

        

        

        

        


        public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
            => await _dbContext.ProductCategories
            .AnyAsync(x => x.Slug == slug && (excludeId == null || x.Id != excludeId));

        public async Task<bool> HasProductsAsync(int categoryId)
            => await _dbContext.Products
            .AnyAsync(x => x.ProductCategoryId == categoryId);

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();



        public async Task<IReadOnlyList<ProductCategory>> SearchAsync(string query, int limit)
        {
            return await _dbContext.ProductCategories
                .AsNoTracking()
                .Where(x => x.Name.Contains(query) || x.Slug.Contains(query))
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
}
