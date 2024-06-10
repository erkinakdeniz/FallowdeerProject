using Core.Persistence.Repositories;

namespace Core.Security.Entities;
public class SCategory:Entity<int>
{
    public string Key { get; set; }
    public string Alias { get; set; }
    public ICollection<Slider> Sliders { get; set; }
}
