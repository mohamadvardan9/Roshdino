using DigitalMarketing.DigitalMarketing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarketing.DigitalMarketing.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(220);
            builder.Property(x => x.ShortDescription).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

            builder.HasIndex(x => x.Slug).IsUnique();

            builder.HasOne(x => x.ProductCategory)
                .WithMany(p => p.Products)
                .HasForeignKey(x => x.ProductCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // اجازه نمیدم که دسته بندی ایی از محصول حذف بشه


            builder.ToTable("Products");
        }
    }
}
