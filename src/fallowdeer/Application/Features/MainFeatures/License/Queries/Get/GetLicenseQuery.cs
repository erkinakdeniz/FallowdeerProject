using Core.CrossCuttingConcerns.Exceptions.Types;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Application.Features.MainFeatures.License.Queries.Get;
public class GetLicenseQuery:IRequest<LicenseResponse>
{
    public class GetLicenseQueryHandler : IRequestHandler<GetLicenseQuery, LicenseResponse>
    {
        private readonly IConfiguration _configuration;

        public GetLicenseQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LicenseResponse> Handle(GetLicenseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string licenseKey = _configuration.GetSection("License").GetSection("Key").Value ?? throw new InvalidOperationException("Lisans key Bulunamadı");
                string apiAddress = _configuration.GetSection("License").GetSection("ApiAddress").Value ?? throw new InvalidOperationException("Lisans adresi Bulunamadı");
                HttpClient httpClient = new() { BaseAddress = new Uri(apiAddress) };
                var queryParams = new Dictionary<string, string>
                {
                    ["licenseKey"] = licenseKey
                };
                var queryString = QueryHelpers.AddQueryString("/api/license", queryParams);
                var response = await httpClient.GetAsync(queryString);
                var json = await response.Content.ReadAsStringAsync();
                LicenseApiResponse licenseApiResponse = JsonConvert.DeserializeObject<LicenseApiResponse>(json);
                LicenseResponse license = new()
                {
                    LicenseExpires = licenseApiResponse.EndingDate,
                    LicenseStartDate = licenseApiResponse.StartingDate,
                    SerialNumber = licenseApiResponse.SerialNumber,
                    IsForever=licenseApiResponse.Forever
                };
                return license;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
           

        }
    }
}
