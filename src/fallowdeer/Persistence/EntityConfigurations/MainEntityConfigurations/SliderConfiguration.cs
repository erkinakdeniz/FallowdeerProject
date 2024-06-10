using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.MainEntityConfigurations;
public class SliderConfiguration : IEntityTypeConfiguration<Slider>
{
    public void Configure(EntityTypeBuilder<Slider> builder)
    {
        builder.ToTable(nameof(Slider) + "s").HasKey(x => x.Id);
        builder.Property(x => x.Visible).HasColumnName("Visible");
        builder.Property(x => x.Title).HasColumnName("Title");
        builder.Property(x => x.Order).HasColumnName("Order");
        builder.Property(x => x.CategoryId).HasColumnName("CategoryId");
        builder.Property(x => x.Description).HasColumnName("Description");
        builder.Property(x => x.Url).HasColumnName("Url");
        builder.Property(x => x.Image).HasColumnName("Image");
        builder.HasQueryFilter(rt => !rt.DeletedDate.HasValue);

        builder.HasOne(x => x.Category).WithMany(x => x.Sliders).HasForeignKey(x => x.CategoryId);
    }
}
