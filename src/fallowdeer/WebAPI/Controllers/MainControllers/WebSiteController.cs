using Application.Features.MainFeatures.Website.Queries.Sliders;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.MainControllers;
/// <summary>
/// Frond End İçin
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class WebSiteController : BaseController
{
   
    [HttpGet("Slider/{CategoryId}")]
    public async Task<IActionResult> GetSliderByCategoryId([FromRoute] SlidesShowQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
