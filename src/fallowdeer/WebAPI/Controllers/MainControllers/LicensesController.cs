using Application.Features.MainFeatures.License.Queries;
using Application.Features.MainFeatures.License.Queries.Get;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.MainControllers;
[Route("api/[controller]")]
[ApiController]
public class LicensesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetLicense()
    {
        var result = await Mediator.Send(new GetLicenseQuery());
        return Ok(result);
    }
    [HttpPost("Control")]
    public async Task<IActionResult> LicenseControl([FromBody] LicenseControlQuery query)
    {
        await Mediator.Send(query);
        return Ok();
    }
}
