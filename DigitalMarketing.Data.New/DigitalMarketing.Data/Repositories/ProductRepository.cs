using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace DigitalMarketing.DigitalMarketing.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _dbContext;
        public ProductRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }






        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetPublishedAsync()
        {
            return await _dbContext.Products
                .Where(p => p.IsPublished)
                .Include(p => p.ProductCategory)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetBySlugAsync(string slug)
        {
            return await _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        }

        public async Task<List<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbContext.Products
                .Where(p => p.ProductCategoryId == categoryId && p.IsPublished)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }






        public async Task AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
        }

        public void Delete(Product product)
        {
            product.IsDeleted = true;
            _dbContext.Products.Update(product);
        }







        public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
        {
            return await _dbContext.Products
                .AnyAsync(p => p.Slug == slug && (excludeId == null || p.Id != excludeId));
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }





        // --- Image Management --- //

        public async Task<ProductImage?> GetImageByIdAsync(int imageId)
        {
            return await _dbContext.ProductImages
                .FirstOrDefaultAsync(i => i.Id == imageId);
        }

        public void RemoveImage(ProductImage image)
        {
            // Hard Delete واقعی، چون فایل فیزیکی هم پاک می‌شه و نگه‌داشتن رکورد فایده‌ای ندارد
            _dbContext.ProductImages.Remove(image);
        }

        public async Task SetMainImageAsync(int productId, int imageId)
        {
            var images = await _dbContext.ProductImages
                .Where(i => i.ProductId == productId)
                .ToListAsync();

            foreach (var img in images)
                img.IsMain = (img.Id == imageId);
        }

    }
}
