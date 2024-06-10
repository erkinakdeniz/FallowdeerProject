using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MainFeatures.WebOptions.Commands.Update.UpdateSlider;
public class UpdateSliderRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public bool Visible { get; set; }
    public int Order { get; set; }
}
