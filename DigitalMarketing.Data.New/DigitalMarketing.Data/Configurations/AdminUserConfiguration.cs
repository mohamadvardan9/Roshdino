using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DigitalMarketing.Data.DigitalMarketing.Data.Configurations
{
    public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> builder)
        {
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PassHash).IsRequired();
            builder.HasIndex(x => x.UserName).IsUnique();


            builder.ToTable("AdminUsers");
        }
    }
}
