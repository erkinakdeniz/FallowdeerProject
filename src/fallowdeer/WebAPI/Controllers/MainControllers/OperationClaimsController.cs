using Application.Features.MainFeatures.OperationClaims.Commands.Create;
using Application.Features.MainFeatures.OperationClaims.Commands.Delete;
using Application.Features.MainFeatures.OperationClaims.Commands.Update;
using Application.Features.MainFeatures.OperationClaims.Queries.GetAll;
using Application.Features.MainFeatures.OperationClaims.Queries.GetById;
using Application.Features.MainFeatures.OperationClaims.Queries.GetList;
using Core.Application.Policies;
using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers.MainControllers;
/// <summary>
/// Yetki Yönetimi
/// </summary>
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(PolicyNames.GeneralFixedPolicyName)]
public class OperationClaimsController : BaseController
{
    /// <summary>
    /// Id'ye Göre Yetki Getir
    /// </summary>
    /// <remarks>
    /// Id'ye göre kayıtlı yetkiyi getirir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdOperationClaimQuery getByIdOperationClaimQuery)
    {
        GetByIdOperationClaimResponse result = await Mediator.Send(getByIdOperationClaimQuery);
        return Ok(result);
    }
    /// <summary>
    /// Tüm Yetkiler
    /// </summary>
    /// <remarks>
    /// Sistemde ki tüm yetkileri getirir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        List<OperationClaimResponse> result = await Mediator.Send(new GetAllOperationClaimsQuery());
        return Ok(result);
    }
    /// <summary>
    /// Tüm Yetkileri Sayfalama Yapısıyla Getir
    /// </summary>
    /// <remarks>
    /// Pagination yapısı vardır.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListOperationClaimQuery getListOperationClaimQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListOperationClaimListItemDto> result = await Mediator.Send(getListOperationClaimQuery);
        return Ok(result);
    }
    /// <summary>
    /// Yetki Ekler
    /// </summary>
    /// <remarks>
    /// Sisteme yeni yetki ekler.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateOperationClaimCommand createOperationClaimCommand)
    {
        CreatedOperationClaimResponse result = await Mediator.Send(createOperationClaimCommand);
        return Created(uri: "", result);
    }
    /// <summary>
    /// Yetki Güncelle
    /// </summary>
    /// <remarks>
    /// Sistemde ki yetkiyi günceller.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommand updateOperationClaimCommand)
    {
        UpdatedOperationClaimResponse result = await Mediator.Send(updateOperationClaimCommand);
        return Ok(result);
    }
    /// <summary>
    /// Yetki Sil
    /// </summary>
    /// <remarks>
    /// Sistemde ki yetkiyi siler
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteOperationClaimCommand deleteOperationClaimCommand)
    {
        DeletedOperationClaimResponse result = await Mediator.Send(deleteOperationClaimCommand);
        return Ok(result);
    }
}
