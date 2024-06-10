using Application.Features.MainFeatures.Library.Commands.Create;
using Application.Features.MainFeatures.Library.Commands.Delete;
using Application.Features.MainFeatures.Library.Commands.Upload;
using Application.Features.MainFeatures.Library.Queries.Files;
using Application.Features.MainFeatures.Library.Queries.Folders;
using Core.Application.Policies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers.MainControllers;
/// <summary>
/// Dosya Yönetimi
/// </summary>
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(PolicyNames.GeneralFixedPolicyName)]
public class LibraryController : BaseController
{
    /// <summary>
    /// Tüm Klasörleri Getir
    /// </summary>
    /// <returns></returns>
    [HttpGet("Folders")]
    public async Task<IActionResult> GetFolders()
    {
        var result = await Mediator.Send(new GetAllLibraryFoldersQuery());
        return Ok(result);
    }
    /// <summary>
    /// Tüm Dosyaları Getir
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    public async Task<IActionResult> GetFiles()
    {
        var result = await Mediator.Send(new GetLibraryFilesQuery());
        return Ok(result);
    }
    /// <summary>
    /// Belirtilen Dizinde ki Dosyaları Getir
    /// </summary>
    /// <param name="getFileQuery"></param>
    /// <returns></returns>
    [HttpGet("Files")]
    public async Task<IActionResult> GetFiles([FromQuery] GetLibraryFileQuery getFileQuery)
    {
        var result = await Mediator.Send(getFileQuery);
        return Ok(result);
    }
    /// <summary>
    /// Popup Üzerinden Dosya Seçimi İçin Gelecek Dosyalar
    /// </summary>
    /// <param name="getFileQuery"></param>
    /// <returns></returns>
    [HttpGet("Files/Popup")]
    public async Task<IActionResult> GetFilesPopup([FromQuery] GetLibraryFileForPopupQuery getFileQuery)
    {
        var result = await Mediator.Send(getFileQuery);
        return Ok(result);
    }
    /// <summary>
    /// Klasör Oluştur
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("CreateFolder")]
    public async Task<IActionResult> CreateFolder([FromBody] CreateLibraryFolderCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Dosyaları Yükle
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("UploadFiles")]
    public async Task<IActionResult> UploadFiles([FromForm] UploadLibraryFileCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Dosya Sil
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("DeleteFile")]
    public async Task<IActionResult> DeleteFile([FromBody] DeleteLibraryFileCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
    /// <summary>
    /// Klasör Sil
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("DeleteFolder")]
    public async Task<IActionResult> DeleteFolder([FromBody] DeleteLibraryFolderCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

}
