using DigitalMarketing.DigitalMarketing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DigitalMarketing.DigitalMarketing.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }



        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<ArticleCategory> ArticleCategories => Set<ArticleCategory>();






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // فیلتر سراسری برای Soft Delete - رکوردهای حذف‌شده تو هیچ Query ای نمیان
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<ProductImage>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<ProductCategory>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Article>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<ArticleCategory>().HasQueryFilter(p => !p.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }




        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }


    }
}
