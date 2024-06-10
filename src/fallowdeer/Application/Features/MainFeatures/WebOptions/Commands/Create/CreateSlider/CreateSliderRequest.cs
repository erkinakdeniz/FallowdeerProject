using Core.Application.Dtos;

namespace Application.Features.MainFeatures.WebOptions.Commands.Create.CreateSlider;
public class CreateSliderRequest:IDto
{
    public CreateSliderRequest()
    {
        Title = "";
        Description = "";
        Url = "";
        Visible = true;
        Order = 0;
    }

    public string? Title { get; set; }
    public string Image { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public bool? Visible { get; set; }
    public int? Order { get; set; }
}
