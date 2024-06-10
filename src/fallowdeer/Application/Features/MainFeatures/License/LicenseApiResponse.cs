using Core.Application.Responses;

namespace Application.Features.MainFeatures.License;
public class LicenseApiResponse:IResponse
{
    public string SerialNumber { get; set; }
    public string LicenseKey { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
    public bool Forever { get; set; }
}
