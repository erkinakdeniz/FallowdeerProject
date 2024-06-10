using Application.Features.MainFeatures.Users.Commands.Create;
using Application.Features.MainFeatures.Users.Commands.Delete;
using Application.Features.MainFeatures.Users.Commands.Update;
using Application.Features.MainFeatures.Users.Commands.Update.ChangePassword;
using Application.Features.MainFeatures.Users.Commands.Update.Password;
using Application.Features.MainFeatures.Users.Commands.Update.Profile;
using Application.Features.MainFeatures.Users.Queries.GetById;
using Application.Features.MainFeatures.Users.Queries.GetList;
using Core.Application.Policies;
using Core.Application.Requests;
using Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers.MainControllers;
/// <summary>
/// Kullan�c� ��lemleri
/// </summary>
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(PolicyNames.GeneralFixedPolicyName)]
public class UsersController : BaseController
{
    /// <summary>
    /// Id'sine G�re Kullan�c� Getir
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response>
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdUserQuery getByIdUserQuery)
    {
        GetByIdUserResponse result = await Mediator.Send(getByIdUserQuery);
        return Ok(result);
    }

    //[HttpGet("GetFromAuth")]
    //public async Task<IActionResult> GetFromAuth()
    //{

    //    GetByIdUserResponse result = await Mediator.Send(new GetByIdUserQuery());
    //    return Ok(result);
    //}

    /// <summary>
    /// Kullan�c�lar� Sayfalama Mant��� ile Getirir
    /// </summary>
    /// <remarks>
    /// Pagination yap�s� var.
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response>
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserQuery getListUserQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListUserListItemDto> result = await Mediator.Send(getListUserQuery);
        return Ok(result);
    }
    /// <summary>
    /// Kullan�c� Ekle
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response>
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
    {

        CreatedUserResponse result = await Mediator.Send(createUserCommand);
        return Created(uri: "", result);
    }
    /// <summary>
    /// Kullan�c� G�ncelle
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand)
    {
        await Mediator.Send(updateUserCommand);
        return Ok();
    }
    /// <summary>
    /// Adminlerin �ifreyi De�i�tirmesini Sa�lar
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response>
    [HttpPost("Update/ChangePassword")]
    public async Task<IActionResult> UpdateChangePassword([FromBody] UpdateUserChangePasswordCommand updateUserCommand)
    {
        await Mediator.Send(updateUserCommand);
        return Ok();
    }
    /// <summary>
    /// Kullan�c�lar Kendi �ifresini De�i�tirir
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response>
    [HttpPost("Update/Password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
    {
        UpdatedUserProfileResponse result = await Mediator.Send(new UpdateUserPasswordCommand() { PasswordDto = updateUserPasswordDto, IpAddress = getIpAddress() });
        return Ok(result);
    }
    /// <summary>
    /// Kullan�c�lar Profilini G�nceller
    /// </summary>
    /// <remarks>
    /// Her kullan�c� kendi profilini g�nceller.
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response>
    [HttpPost("Update/Profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto user)
    {

        var result = await Mediator.Send(new UpdateUserProfileCommand() { User = user, IpAddress = getIpAddress() });
        return Ok(result);
    }
    /// <summary>
    /// Kullan�c� Sil
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Ba�ar�l�</response>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteUserCommand deleteUserCommand)
    {
        DeletedUserResponse result = await Mediator.Send(deleteUserCommand);
        return Ok(result);
    }
}
