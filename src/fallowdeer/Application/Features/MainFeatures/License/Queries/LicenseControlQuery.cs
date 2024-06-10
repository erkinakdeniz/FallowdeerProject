using Core.Application.Pipelines.License;
using Core.CrossCuttingConcerns.Exceptions.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Text;

namespace Application.Features.MainFeatures.License.Queries;
public class LicenseControlQuery:IRequest
{
    public string Key { get; set; }
    public class LicenseControlQueryHandler : IRequestHandler<LicenseControlQuery>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public LicenseControlQueryHandler(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
        }

        public async Task Handle(LicenseControlQuery request, CancellationToken cancellationToken)
        {
            string host = _httpContext.HttpContext.Request.Host.Value;
            string hostName = host.Split(":")[0];
            
            string apiAddress = _configuration.GetSection("License").GetSection("ApiAddress").Value ?? throw new InvalidOperationException("Lisans adresi Bulunamadı");
            HttpClient httpClient = new() { BaseAddress = new Uri(apiAddress) };

            string user_agent = _httpContext.HttpContext.Request.Headers.UserAgent.ToString();

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(user_agent);
            httpClient.DefaultRequestHeaders.Add("Cookie",
                $"host={_httpContext.HttpContext.Request.Host.Value};" +
                $"ip={_httpContext.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString()};" +
                $"mac={(from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where nic.OperationalStatus == OperationalStatus.Up
                        select nic.GetPhysicalAddress()).FirstOrDefault().ToString()};");


            var jsonBody = "{\r\n  \"licenseKey\": \"" + request.Key + "\"\r\n}";
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync("/api/license/control", content);
                if (!response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ProblemDetails>(json);
                    throw new BusinessException(result.Detail);
                }
               
            }
            catch (Exception ex)
            {

                throw new BusinessException($"{ex.Message}");
            }
        }
    }
}
