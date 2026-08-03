using DigitalMarketing.DigitalMarketing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalMarketing.DigitalMarketing.Data.Configurations
{
    public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
    {
        public void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);

            builder.Property(x => x.Slug).IsRequired().HasMaxLength(180);
            builder.HasIndex(x => x.Slug).IsUnique();

            builder.ToTable("ArticleCategories");

        }
    }
}
