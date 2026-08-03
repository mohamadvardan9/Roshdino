using DigitalMarketing.DigitalMarketing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarketing.DigitalMarketing.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500);


            builder.HasOne(x => x.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);  // اگر پروداکتی حذف شد, عکس هایش هم حذف شوند

            builder.ToTable("ProductImages");
        }
    }
}
