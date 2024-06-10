using Core.Application.Responses;

namespace Application.Features.MainFeatures.License.Queries;
public class LicenseResponse:IResponse
{
    public string SerialNumber { get; set; }
    public DateTime LicenseStartDate { get; set; }
    public DateTime LicenseExpires { get; set; }
    public bool IsForever { get; set; }
}
