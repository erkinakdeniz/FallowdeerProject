using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations.MainEntityConfigurations;
public class WebOptionConfiguration : IEntityTypeConfiguration<WebOption>
{
    public void Configure(EntityTypeBuilder<WebOption> builder)
    {
        builder.ToTable("WebOptions").HasKey(x => x.Id);
        builder.Property(x => x.Key).HasColumnName("Key").IsRequired();
        builder.Property(x => x.Value).HasColumnName("Value").IsRequired();
        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);
        builder.HasData(getSeeds());
    }
    private IEnumerable<WebOption> getSeeds()
    {
        var webOptions = new List<WebOption>();
        int id = 0;
        
        var webOption2 = new WebOption()
        {
            Id = ++id,
            Key = "SiteName",
            Value = "Kodkop Teknoloji",
            Alias = "Sitenin Başlığı",
            InputType = "Text"
        };

        var webOption3 = new WebOption()
        {
            Id = ++id,
            Key = "SiteDescription",
            Value = "Erkin ve Başar",
            Alias = "Site Açıklaması",
            InputType = "Text"
        };
        var webOption4 = new WebOption()
        {
            Id = ++id,
            Key = "ExternalRegistration",
            Value = "True",
            Alias = "Üye Kayıt",
            InputType = "Select"
        };
        var webOption5 = new WebOption()
        {
            Id = ++id,
            Key = "HeaderLogo",
            Value = "",
            Alias = "Üst Başlık Logosu",
            InputType = "Image",
            Params = """{"Width":160,"Height":90}"""

        };
        var webOption6 = new WebOption()
        {
            Id = ++id,
            Key = "FooterLogo",
            Value = "",
            Alias = "Footer Logosu",
            InputType = "Image",
            Params = """{"Width":192,"Height":108}"""
        };
        var webOption7 = new WebOption()
        {
            Id = ++id,
            Key = "FooterDescription",
            Value = "",
            Alias = "Footer Açıklaması",
            InputType = "Text"
        };
        webOptions.Add(webOption2);
        webOptions.Add(webOption3);
        webOptions.Add(webOption4);
        webOptions.Add(webOption5);
        webOptions.Add(webOption6);
        webOptions.Add(webOption7);
        return webOptions;
    }
}
