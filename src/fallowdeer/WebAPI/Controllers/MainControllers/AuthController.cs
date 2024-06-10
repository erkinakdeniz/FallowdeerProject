using Application.Features.MainFeatures.Auth.Commands.DisableEmailAuthenticator;
using Application.Features.MainFeatures.Auth.Commands.DisableOtpAuthenticator;
using Application.Features.MainFeatures.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.MainFeatures.Auth.Commands.EnableOtpAuthenticator;
using Application.Features.MainFeatures.Auth.Commands.ExternalRegistrationControl;
using Application.Features.MainFeatures.Auth.Commands.ForgotPassword;
using Application.Features.MainFeatures.Auth.Commands.Login;
using Application.Features.MainFeatures.Auth.Commands.Login.EmailLogin;
using Application.Features.MainFeatures.Auth.Commands.Login.Login;
using Application.Features.MainFeatures.Auth.Commands.Login.OTPLogin;
using Application.Features.MainFeatures.Auth.Commands.PageGuard;
using Application.Features.MainFeatures.Auth.Commands.RefreshToken;
using Application.Features.MainFeatures.Auth.Commands.Register;
using Application.Features.MainFeatures.Auth.Commands.ResetPassword;
using Application.Features.MainFeatures.Auth.Commands.RevokeToken;
using Application.Features.MainFeatures.Auth.Commands.VerifyCode;
using Application.Features.MainFeatures.Auth.Commands.VerifyEmailAuthenticator;
using Application.Features.MainFeatures.Auth.Commands.VerifyOtpAuthenticator;
using Application.Features.MainFeatures.Auth.Queries.AuthenticatorTypes;
using Core.Application.Dtos;
using Core.Application.Policies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;
using static Application.Features.MainFeatures.Auth.Commands.Login.LoggedResponse;

namespace WebAPI.Controllers.MainControllers;
/// <summary>
/// Kullanıcı Kayıt ve Giriş İşlemleri
/// </summary>
[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(PolicyNames.AuthFixedPolicyName)]
public class AuthController : BaseController
{
    private readonly WebApiConfiguration _configuration;
    /// <summary>
    /// Constractor
    /// </summary>
    /// <param name="configuration"></param>
    /// <exception cref="NullReferenceException"></exception>
    public AuthController(IConfiguration configuration)
    {
        const string configurationSection = "WebAPIConfiguration";
        _configuration =
            configuration.GetSection(configurationSection).Get<WebApiConfiguration>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
    }

    //[HttpGet("test")]
    //public async Task<IActionResult> Test()
    //{
    //   var result= await Mediator.Send(new TestCommand());
    //    return Ok(result);
    //}



    #region Login
    /// <summary>
    /// Sisteme Giriş
    /// </summary>
    /// <remarks>
    /// Giriş işlemi sırasında lisans kontrol edilir
    /// </remarks>
    /// <param name="userForLoginDto"></param>
    /// <returns>Giriş Bilgileri</returns>
    /// <response code="200">Token bilgileri aşağıda ki gibidir</response>
    /// <response code="400">Doğrulama işlemleri ve girilen bilgiler yanlış ise BusinessException çalışır</response>  
    [HttpPost("Login")]
    [ProducesResponseType(typeof(LoggedHttpResponse), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
    {
        var sw = Stopwatch.StartNew();
        LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto, IpAddress = getIpAddress() };
        LoggedResponse result = await Mediator.Send(loginCommand);

        if (result.RefreshToken is not null)
            setRefreshTokenToHeader(result.RefreshToken.Token);
        Response.Headers["X-Response-Time"] = sw.Elapsed.TotalMilliseconds.ToString() + "Ms";

        return Ok(result.ToHttpResponse());
    }
    /// <summary>
    /// Sisteme Mail İle Giriş
    /// </summary>
    /// <remarks>
    /// url: /api/auth/login/email
    /// </remarks>
    /// <param name="emailLoginDto"></param>
    /// <returns>Giriş Bilgileri</returns>
    /// <response code="200">Token bilgileri aşağıda ki gibidir</response>
    /// <response code="400">Doğrulama işlemleri ve girilen bilgiler yanlış ise BusinessException çalışır</response>  
    [HttpPost("Login/Email")]
    [ProducesResponseType(typeof(LoggedHttpResponse), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> EmailLogin([FromBody] EmailLoginDto emailLoginDto)
    {
        var sw = Stopwatch.StartNew();
        EmailLoginCommand loginCommand = new() { Email = emailLoginDto.Email, Code = emailLoginDto.Code, IpAddress = getIpAddress() };
        LoggedResponse result = await Mediator.Send(loginCommand);

        if (result.RefreshToken is not null)
            setRefreshTokenToHeader(result.RefreshToken.Token);
        Response.Headers["X-Response-Time"] = sw.Elapsed.TotalMilliseconds.ToString() + "Ms";

        return Ok(result.ToHttpResponse());
    }
    /// <summary>
    /// Sisteme Tek Kullanımlık Şifre İle Giriş
    /// </summary>
    /// <remarks>
    /// url: /api/auth/login/otp
    /// </remarks>
    /// <param name="otpLogin"></param>
    /// <returns>Giriş Bilgileri</returns>
    /// <response code="200">Token bilgileri aşağıda ki gibidir</response>
    /// <response code="400">Doğrulama işlemleri ve girilen bilgiler yanlış ise BusinessException çalışır</response>  
    [HttpPost("Login/Otp")]
    [ProducesResponseType(typeof(LoggedHttpResponse), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> OtpLogin([FromBody] OtpLoginCommandDto otpLogin)
    {
        var sw = Stopwatch.StartNew();
        OTPLoginCommand loginCommand = new() { AuthenticatorCode = otpLogin.AuthenticatorCode, Email = otpLogin.Email, IpAddress = getIpAddress() };
        LoggedResponse result = await Mediator.Send(loginCommand);

        if (result.RefreshToken is not null)
            setRefreshTokenToHeader(result.RefreshToken.Token);
        Response.Headers["X-Response-Time"] = sw.Elapsed.TotalMilliseconds.ToString() + "Ms";

        return Ok(result.ToHttpResponse());
    }
    #endregion

    /// <summary>
    /// Dışarıdan Kayıt
    /// </summary>
    /// <remarks>
    /// url: /api/auth/register
    /// </remarks>
    /// <param name="userForRegisterDto"></param>
    /// <returns>Token</returns>
    /// <response code="200">Token bilgileri aşağıda ki gibidir</response>
    /// <response code="400">Doğrulama işlemleri ve girilen bilgiler yanlış ise BusinessException çalışır</response>  
    [HttpPost("Register")]
    [ProducesResponseType(typeof(LoggedHttpResponse), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    {
        RegisterCommand registerCommand = new() { UserForRegisterDto = userForRegisterDto, IpAddress = getIpAddress() };
        RegisteredResponse result = await Mediator.Send(registerCommand);
        //setRefreshTokenToCookie(result.RefreshToken);
        return Ok(result.ToHttpResponse());
    }
    [HttpPost("ercontrol")]
    public async Task<IActionResult> ExternalRegistrationControl()
    {
        await Mediator.Send(new ExternalRegistrationControlCommand());
        return Ok();
    }
    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <remarks>
    /// url: /api/auth/referer
    /// </remarks>
    /// <response code="200">Refresh Token bilgileri aşağıda ki gibidir</response>
    /// <response code="400">Refresh Token geçersizse hata verir</response>  
    [HttpPost("Referer")]
    [ProducesResponseType(typeof(RefreshTokenDto), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken()
    {
        string refreshToken = getRefreshTokenFromHeader();
        RefreshTokenCommand refreshTokenCommand = new() { RefreshToken = refreshToken, IpAddress = getIpAddress() };
        RefreshedTokensResponse result = await Mediator.Send(refreshTokenCommand);
        setRefreshTokenToHeader(result.Referer.Token);
        return Ok(result.ToHttpResponse());
    }
    /// <summary>
    /// Refresh Token Silme
    /// </summary>
    /// <remarks>
    /// url: /api/auth/revoketoken
    /// </remarks>
    /// <param name="refreshToken"></param>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Refresh Token silme işlemi başarısız ise</response>  
    [HttpPost("RevokeToken")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> RevokeToken([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string? refreshToken)
    {
        RevokeTokenCommand revokeTokenCommand = new() { Token = refreshToken ?? getRefreshTokenFromHeader(), IpAddress = getIpAddress() };
        RevokedTokenResponse result = await Mediator.Send(revokeTokenCommand);
        return Ok();
    }

    #region Authenticator
    /// <summary>
    /// Email Doğrulamayı Aktif et
    /// </summary>
    /// <remarks>
    /// url: /api/auth/enableemailauthenticator
    /// </remarks>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Kullanıcı yoksa veya hata oluşursa</response>  
    [HttpGet("EnableEmailAuthenticator")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> EnableEmailAuthenticator()
    {
        await Mediator.Send(new EnableEmailAuthenticatorCommand());

        return Ok();
    }
    /// <summary>
    /// Email Doğrulama Devredışı Bırakılır
    /// </summary>
    /// <remarks>
    /// url: /api/auth/enableemailauthenticator
    /// </remarks>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Kullanıcı yoksa veya hata oluşursa</response>  
    [HttpPost("DisableEmailAuthenticator")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> DisableEmailAuthenticator()
    {
        await Mediator.Send(new DisableEmailAuthenticatorCommand());
        return Ok();
    }
    /// <summary>
    /// Tek Kullanımlık Şifre Doğrulamayı Aktif Et
    /// </summary>
    /// <remarks>
    /// url: /api/auth/enableotpauthenticator
    /// </remarks>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Kullanıcı yoksa veya hata oluşursa</response> 
    [HttpGet("EnableOtpAuthenticator")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> EnableOtpAuthenticator()
    {

        await Mediator.Send(new EnableOtpAuthenticatorCommand());

        return Ok();
    }
    /// <summary>
    /// Tek Kullanımlık Şifre Doğrulamayı Devredışı Bırak
    /// </summary>
    /// <remarks>
    /// url: /api/auth/disableotpauthenticator
    /// </remarks>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Kullanıcı yoksa veya hata oluşursa</response> 
    [HttpPost("DisableOtpAuthenticator")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> DisableOtpAuthenticator()
    {

        await Mediator.Send(new DisableOtpAuthenticatorCommand());
        return Ok();
    }
    /// <summary>
    /// Email Doğrula
    /// </summary>
    /// <remarks>
    /// url: /api/auth/verifyemailauthenticator
    /// </remarks>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Kullanıcı yoksa veya hata oluşursa</response> 
    [HttpGet("VerifyEmailAuthenticator")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyEmailAuthenticator([FromQuery] VerifyEmailAuthenticatorCommand verifyEmailAuthenticatorCommand)
    {
        await Mediator.Send(verifyEmailAuthenticatorCommand);
        return Ok();
    }
    /// <summary>
    /// Tek Kullanımlık Şifre Doğrulama
    /// </summary>
    /// <remarks>
    /// url: /api/auth/verifyotpauthenticator
    /// </remarks>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Kullanıcı yoksa veya hata oluşursa</response> 
    [HttpPost("VerifyOtpAuthenticator")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyOtpAuthenticator([FromBody] VerifyOtpAuthenticatorDto verifyOtp)
    {
        VerifyOtpAuthenticatorCommand verifyEmailAuthenticatorCommand =
            new() { ActivationCode = verifyOtp.ActivationKey };

        await Mediator.Send(verifyEmailAuthenticatorCommand);
        return Ok();
    }
    #endregion
    /// <summary>
    /// Sayfa Güvenliği
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <response code="200">Başarılı</response>
    /// <response code="400">Token Geçersizse</response> 
    [HttpGet()]   
    [DisableRateLimiting]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> PageGuard()
    {
        string token = Request.Headers["Authorization"].ToString();
        string refreshToken = Request.Headers["Referer-Ux"].ToString();
        bool tokenVerify=  await Mediator.Send(new PageGuardCommand() { Token=token});
        if (!tokenVerify)
        {
          var result= await Mediator.Send(new RefreshTokenCommand() { IpAddress = getIpAddress(), RefreshToken = refreshToken });
          return Ok(result.ToHttpResponse());
        }
        return Ok();
    }
    /// <summary>
    /// Şifremi Unuttum Butonu İçin
    /// </summary>
    /// <remarks>
    /// Kullanıcı şifresini unuttuğunda kullanılır. Kullanıcıya E-Posta olarak Kod gönderilir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Forgotpassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordUserControlCommand command)
    {
        var userInfo = await Mediator.Send(command);
        if (userInfo is not null)
            await Mediator.Send(new ForgotPasswordCommand() { Email = userInfo.Email, Code = userInfo.Code });
        return Ok();
    }
    /// <summary>
    /// Şifre Sıfırla
    /// </summary>
    /// <remarks>
    /// Kullanıcı şifresini sıfırlamak istediğinde kullanılır.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("Resetpassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }
    /// <summary>
    /// Kod Doğrulama İşlemi
    /// </summary>
    /// <remarks>
    /// E-Posta ve kod gönderilerek sistemde eşleşme sağlanıp sağlanmadığı bakılır.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpPost("VerifyCode")]
    public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }
    /// <summary>
    /// Doğrulama Tiplerini Getir
    /// </summary>
    /// <remarks>
    /// Kullanıcının doğrulama tiplerini getirir.
    /// </remarks>
    /// <response code="200">Başarılı</response>
    [HttpGet("AuthenticatorTypes")]
    public async Task<IActionResult> AuthenticatorTypes()
    {
        var result = await Mediator.Send(new GetAuthenticatorTypesQuery());
        return Ok(result);
    }
}
