namespace WebAPI;

public class WebApiConfiguration
{
    public string ApiDomain { get; set; }
    public string Domain { get; set; }
    public string[] AllowedOrigins { get; set; }

    public WebApiConfiguration()
    {
        ApiDomain = string.Empty;
        AllowedOrigins = Array.Empty<string>();
        Domain = string.Empty;
    }

    public WebApiConfiguration(string apiDomain, string domain, string[] allowedOrigins)
    {
        ApiDomain = apiDomain;
        Domain = domain;
        AllowedOrigins = allowedOrigins;
    }
}
