using Application.Features.MainFeatures.WebOptions.Commands.Create;
using Application.Features.MainFeatures.WebOptions.Commands.Create.CreateSlider;
using Application.Features.MainFeatures.WebOptions.Commands.Delete;
using Application.Features.MainFeatures.WebOptions.Commands.Delete.DeleteSlider;
using Application.Features.MainFeatures.WebOptions.Commands.SaveHeaderLogo;
using Application.Features.MainFeatures.WebOptions.Commands.Update;
using Application.Features.MainFeatures.WebOptions.Commands.Update.UpdateSlider;
using Application.Features.MainFeatures.WebOptions.Queries.SliderCategory;
using Application.Features.MainFeatures.WebOptions.Queries.Sliders;
using Application.Features.MainFeatures.WebOptions.Queries.WebOptions;
using Core.Application.Policies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers.MainControllers;
/// <summary>
/// Web Sitesi Ayarları
/// </summary>
[Route("api/Web")]
[ApiController]
[EnableRateLimiting(PolicyNames.GeneralFixedPolicyName)]
public class WebOptionsController : BaseController
{

    /// <summary>
    /// Tüm Ayarları Getir
    /// </summary>
    /// <remarks>
    /// Sistemde ki tüm ayarları getirir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetWebOptions()
    {
        var result = await Mediator.Send(new GetWebOptionsQuery());
        return Ok(result);
    }
    /// <summary>
    /// Yeni Ayar Oluştur
    /// </summary>
    /// <remarks>
    /// Sisteme yeni ayar oluşturur.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateWebOptionCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Belirtilen Ayarı Sil
    /// </summary>
    /// <remarks>
    /// Sistemde ki belirtilen ayarı siler
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteWebOptionCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Belirtilen Ayarı Güncelle
    /// </summary>
    /// <remarks>
    /// Sistemde ki ayarı günceller
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateWebOptionCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Sisteme Resim Kaydeder
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("SaveImages")]
    public async Task<IActionResult> SaveImages([FromForm] SaveWebOptionImageCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    #region slider
    /// <summary>
    /// Tüm Slaytları Getirir
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("Slider/GetAll")]
    public async Task<IActionResult> SliderGetAll()
    {
        var result = await Mediator.Send(new GetAllSliderQuery());
        return Ok(result);
    }
    /// <summary>
    /// Slayt Id'sine Göre Slayt Getirir
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("Slider/{Id}")]
    public async Task<IActionResult> SliderGetById([FromRoute] GetSliderQuery command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    /// <summary>
    /// Slaytın Kategorisine Göre Slaytları Getirir
    /// </summary>
    /// <remarks>
    /// Kategori Id'sine Göre slaytlar gelir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("Slider/ByCategory/{CategoryId}")]
    public async Task<IActionResult> SliderGetByCategoryId([FromRoute] GetByCategorySlidesQuery command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
    /// <summary>
    /// Toplu Slayt Oluştur
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Slider/Add")]
    public async Task<IActionResult> CreateSlider([FromBody] CreateSliderCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Slayt Sil
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Slider/Delete")]
    public async Task<IActionResult> DeleteSlider([FromBody] DeleteSliderCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Toplu Slayt Güncelle
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Slider/Update")]
    public async Task<IActionResult> UpdateSlider([FromBody] UpdateSliderCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Slayt Kategorilerini Getirir.
    /// </summary>
    /// <remarks>
    /// Slayt için olan kategoriler gelir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("Slider/Category")]
    public async Task<IActionResult> GetSilderCategory()
    {
        var result = await Mediator.Send(new GetSCategoriesQuery());
        return Ok(result);
    }
    #endregion

}