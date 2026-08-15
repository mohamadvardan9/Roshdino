using DigitalMarketing.Core.DigitalMarketing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMarketing.Data.DigitalMarketing.Data.Configurations
{
    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Email).HasMaxLength(250);
            builder.Property(x => x.Phone).HasMaxLength(11);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(2000);

            builder.ToTable("ContactMessages");
        }
    }
}
