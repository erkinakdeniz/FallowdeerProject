using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.MainEntityConfigurations;
public class SCategoryConfiguration : IEntityTypeConfiguration<SCategory>
{
    public void Configure(EntityTypeBuilder<SCategory> builder)
    {
        builder.ToTable("SCategories").HasKey(x => x.Id);
        builder.Property(x => x.Alias).HasColumnName("Alias");
        builder.Property(x => x.Key).HasColumnName("Key");
        builder.HasQueryFilter(rt => !rt.DeletedDate.HasValue);
        builder.HasMany(x => x.Sliders).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId);
        builder.HasData(getSeeds());

    }
    private IEnumerable<SCategory> getSeeds()
    {
        List<SCategory> sCategories = new();
        var sCategory = new SCategory();
        sCategory.Key = "Home";
        sCategory.Alias = "Ana Sayfa";
        sCategory.Id = 1;
        var sCategory2 = new SCategory();
        sCategory2.Key = "Tag";
        sCategory2.Alias = "Künye";
        sCategory2.Id = 2;
        var sCategory3 = new SCategory();
        sCategory3.Key = "Contact";
        sCategory3.Alias = "İletişim";
        sCategory3.Id = 3;
        sCategories.Add(sCategory);
        sCategories.Add(sCategory2);
        sCategories.Add(sCategory3);
        return sCategories;
    }
}
