using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.EntityConfigurations.MainEntityConfigurations;
public class UnicodeConfiguration : IEntityTypeConfiguration<Unicode>
{
    public void Configure(EntityTypeBuilder<Unicode> builder)
    {
        builder.ToTable("Unicodes").HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("Id");
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.Email).HasColumnName("Email");
        builder.Property(x => x.ExpiredDate).HasColumnName("ExpiredDate").IsRequired();
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(x => x.DeletedDate).HasColumnName("DeletedDate");
        builder.HasQueryFilter(x => !x.DeletedDate.HasValue);
    }
}
