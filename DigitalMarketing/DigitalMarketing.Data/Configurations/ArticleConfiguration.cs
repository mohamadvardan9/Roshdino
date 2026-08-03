using DigitalMarketing.DigitalMarketing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarketing.DigitalMarketing.Data.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(220);
            builder.Property(x => x.Summary).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.CoverImageUrl).HasMaxLength(500);


            builder.HasIndex(x => x.Slug).IsUnique();


            builder.HasOne(x => x.ArticleCategory)
                .WithMany(a => a.Articles)
                .HasForeignKey(x => x.ArticleCategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.ToTable("Articles");
        }
    }
}
