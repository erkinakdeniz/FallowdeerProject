using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.WebOptions.Queries.Sliders;
public class SliderQueryResponse
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string Image { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string Category { get; set; }
    public bool Visible { get; set; }
    public int Order { get; set; }
}
