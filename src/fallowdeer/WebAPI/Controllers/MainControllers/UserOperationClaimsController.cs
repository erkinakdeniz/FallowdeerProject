using Application.Features.MainFeatures.UserOperationClaims.Commands.Create;
using Application.Features.MainFeatures.UserOperationClaims.Commands.Delete;
using Application.Features.MainFeatures.UserOperationClaims.Commands.Update;
using Application.Features.MainFeatures.UserOperationClaims.Queries.GetById;
using Application.Features.MainFeatures.UserOperationClaims.Queries.GetByUserId;
using Application.Features.MainFeatures.UserOperationClaims.Queries.GetList;
using Core.Application.Policies;
using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers.MainControllers;
/// <summary>
/// Kullanıcı Yetkileri
/// </summary>
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(PolicyNames.GeneralFixedPolicyName)]
public class UserOperationClaimsController : BaseController
{
    /// <summary>
    /// Id'ye Göre Kullanıcı Yetkisi Getir
    /// </summary>
    /// <remarks>
    /// Kullanıcının yetkisini getirir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdUserOperationClaimQuery getByIdUserOperationClaimQuery)
    {
        GetByIdUserOperationClaimResponse result = await Mediator.Send(getByIdUserOperationClaimQuery);
        return Ok(result);
    }
    /// <summary>
    /// Kullanısının Id'sine Göre Kullanıcın Yetkisini Getir
    /// </summary>
    /// <remarks>
    /// Kullanıcının yetkisini getirir
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("Get/{UserId}")]
    public async Task<IActionResult> GetByUserId([FromRoute] GetByUserIdUserOperationClaimsQuery query)
    {
        List<GetByUserIdUserOperationClaimResponse> result = await Mediator.Send(query);
        return Ok(result);
    }
    /// <summary>
    /// Sayfalama Mantığı ile Kullanıcıların Yetkilerini Getir
    /// </summary>
    /// <remarks>
    /// Pagination yapısı kullanılır.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserOperationClaimQuery getListUserOperationClaimQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListUserOperationClaimListItemDto> result = await Mediator.Send(getListUserOperationClaimQuery);
        return Ok(result);
    }
    /// <summary>
    /// Kullanıcıya Yetki Ekle
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserOperationClaimCommand createUserOperationClaimCommand)
    {
        CreatedUserOperationClaimResponse result = await Mediator.Send(createUserOperationClaimCommand);
        return Created(uri: "", result);
    }
    /// <summary>
    /// Kullanıcının Yetkisini Güncelle
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserOperationClaimCommand updateUserOperationClaimCommand)
    {
        UpdatedUserOperationClaimResponse result = await Mediator.Send(updateUserOperationClaimCommand);
        return Ok(result);
    }
    /// <summary>
    /// Kullanıcının Yetkisini Siler
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteUserOperationClaimCommand deleteUserOperationClaimCommand)
    {
        DeletedUserOperationClaimResponse result = await Mediator.Send(deleteUserOperationClaimCommand);
        return Ok(result);
    }
}
