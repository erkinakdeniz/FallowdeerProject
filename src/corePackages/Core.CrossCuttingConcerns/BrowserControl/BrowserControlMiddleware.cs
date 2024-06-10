using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.CrossCuttingConcerns.Extensions;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Core.CrossCuttingConcerns.BrowserControl;
public class BrowserControlMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly LoggerServiceBase _loggerServiceBase;
    private readonly IHttpContextAccessor _contextAccessor;

    public BrowserControlMiddleware(RequestDelegate next, IConfiguration configuration, LoggerServiceBase loggerServiceBase, IHttpContextAccessor contextAccessor)
    {
        _next = next;
        _configuration = configuration;
        _loggerServiceBase = loggerServiceBase;
        _contextAccessor = contextAccessor;
    }
    public async Task InvokeAsync(HttpContext context)
    {
       var userAgent= context.Request.Headers["User-Agent"].FirstOrDefault()?.Split("/");
        foreach (string s in userAgent)
        {
            bool contains = browserList.Contains(s);
            if (contains)
            {
              var domain=  _configuration.GetSection("WebAPIConfiguration").GetSection("Domain").Value;
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Content-Security-Policy", $"frame-ancestors 'self' {domain}");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                await _next(context);
                return;
            }
        }
        List<LogParameter> logParameters =
        new()
            {
                new LogParameter { Type = context.GetType().Name, Value = $"{userAgent[0] ?? "Bilinmeyen kişi"} tarafından giriş yapılmaya Çalışıldı!" }
            };

        LogDetail logDetail =
            new()
            {
                MethodName = _next.Method.Name,
                Parameters = logParameters,
                User = _contextAccessor.HttpContext.Items[MyClaimTypes.Name]?.ToString() ?? "?",
                Email = _contextAccessor.HttpContext.Items[MyClaimTypes.Email]?.ToString() ?? "?",
                UserId = Guid.Parse(_contextAccessor.HttpContext?.Items[MyClaimTypes.ID]?.ToString() ?? "00000000-0000-0000-0000-000000000000"),
                IPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() ?? "000.000.000.000",
            };

        _loggerServiceBase.Warn(JsonSerializer.Serialize(logDetail));
        throw new AuthorizationException(SecurityMessages.AuthorizationException);

        
    }
    private static string[] browserList = new[] { "ABrowse", "Acoo Browser", "America Online Browser", "AmigaVoyager", "AOL", "Arora", "Avant Browser", "Beonex", "BonEcho", "Browzar", "Camino", "Charon", "Cheshire", "Chimera", "Chrome", "ChromePlus", "Classilla", "CometBird", "Comodo_Dragon", "Conkeror", "Crazy Browser", "Cyberdog", "Deepnet Explorer", "DeskBrowse", "Dillo", "Dooble", "Edge", "Element Browser", "Elinks", "Enigma Browser", "EnigmaFox", "Epiphany", "Escape", "Firebird", "Firefox", "Fireweb Navigator", "Flock", "Fluid", "Galaxy", "Galeon", "GranParadiso", "GreenBrowser", "Hana", "HotJava", "IBM WebExplorer", "IBrowse", "iCab", "Iceape", "IceCat", "Iceweasel", "iNet Browser", "Internet Explorer", "iRider", "Iron", "K-Meleon", "K-Ninja", "Kapiko", "Kazehakase", "Kindle Browser", "KKman", "KMLite", "Konqueror", "LeechCraft", "Links", "Lobo", "lolifox", "Lorentz", "Lunascape", "Lynx", "Madfox", "Maxthon", "Midori", "Minefield", "Mozilla", "myibrow", "MyIE2", "Namoroka", "Navscape", "NCSA_Mosaic", "NetNewsWire", "NetPositive", "Netscape", "NetSurf", "OmniWeb", "Opera", "Orca", "Oregano", "osb-browser", "Palemoon", "Phoenix", "Pogo", "Prism", "QtWeb Internet Browser", "Rekonq", "retawq", "RockMelt", "Safari", "SeaMonkey", "Shiira", "Shiretoko", "Sleipnir", "SlimBrowser", "Stainless", "Sundance", "Sunrise", "surf", "Sylera", "Tencent Traveler", "TenFourFox", "theWorld Browser", "uzbl", "Vimprobable", "Vonkeror", "w3m", "WeltweitimnetzBrowser", "WorldWideWeb", "Wyzo" };

   
}
public static class BrowserControlMiddlewareExtensions
{
    public static IApplicationBuilder UseBrowserControl(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BrowserControlMiddleware>();
    }
}
