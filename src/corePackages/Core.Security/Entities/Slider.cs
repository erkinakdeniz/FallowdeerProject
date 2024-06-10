using Core.Persistence.Repositories;

namespace Core.Security.Entities;
public class Slider:Entity<int>
{
   
    public Slider()
    {
        Title = string.Empty;
        Image= string.Empty;
        Description = string.Empty;
        Url = string.Empty;
        Order = 0;
    }

    public Slider(string title, string ımage, string? description, string? url, int categoryId, bool visible, int order)
    {
        Title = title;
        Image = ımage;
        Description = description;
        Url = url;
        CategoryId = categoryId;
        Visible = visible;
        Order = order;
    }

    public string Title { get; set; }
    public string Image { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public int CategoryId { get; set; } = 1;
    public bool Visible { get; set; }=true;
    public int Order { get; set; }
    public SCategory Category { get; set; }
   
}
