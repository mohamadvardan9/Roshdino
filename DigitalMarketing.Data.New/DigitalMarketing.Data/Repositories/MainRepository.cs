using DigitalMarketing.Core.DigitalMarketing.Core.Interfaces;
using DigitalMarketing.DigitalMarketing.Core.Entities;
using DigitalMarketing.DigitalMarketing.Data;
using Microsoft.EntityFrameworkCore;


namespace DigitalMarketing.Data.DigitalMarketing.Data.Repositories
{
    public class MainRepository : IMainRepository
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;

        // نکته خیلی مهم:
        // برخلاف بقیه Repository های پروژه که DbContext رو مستقیم Inject می‌کنن،
        // این کلاس از IDbContextFactory استفاده می‌کنه چون متدهاش قراره
        // به‌صورت Parallel (با Task.WhenAll) در Service صدا زده بشن.
        // چون یک نمونه DbContext مشترک Thread-Safe نیست و اجازه دو عملیات
        // همزمان رو نمی‌ده، هر متد اینجا با CreateDbContextAsync() یک
        // DbContext مستقل و کوتاه‌عمر برای خودش می‌سازه.
        public MainRepository(IDbContextFactory<MyDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }





        public async Task<int> GetArticlesCountAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Articles.CountAsync();
        }
        public async Task<int> GetArticlesCountSinceAsync(DateTime since)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Articles.CountAsync(a => a.CreatedAt >= since);
        }

        public async Task<int> GetDraftArticlesCountAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Articles.CountAsync(x => !x.IsPublished);
        }

        public async Task<List<Article>> GetLatestArticlesAsync(int count)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Articles
            .Include(a => a.ArticleCategory)
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .ToListAsync();
        }






        public async Task<int> GetProductsCountAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Products.CountAsync();
        }

        public async Task<int> GetProductsCountSinceAsync(DateTime since)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Products.CountAsync(p => p.CreatedAt >= since);
        }

        public async Task<int> GetDraftProductsCountAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Products.CountAsync(x => !x.IsPublished);
        }

        public async Task<List<Product>> GetLatestProductsAsync(int count)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Products
            .Include(p => p.ProductCategory)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
        }







        public async Task<int> GetUnreadMessagesCountAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.ContactMessages
            .OrderByDescending(cm => cm.CreatedAt)
            .CountAsync(cm => !cm.IsRead);
        }




    }
}
